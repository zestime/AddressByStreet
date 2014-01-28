
#Background 배경#

lucene은 hadoop의 아버지 더그 컷팅이라는 사람이 2005년에 만든 오픈 소스 검색 엔진이다. 그냥 작은 구글같은거라고 생각하면 된다. 무언가를 찾을 때, 쉽게 쓸 수 있도록 정리해 놓는 도구이다. 이를테면, 도서관의 십진분류법에 따라 정리해주는 사서와 같은 역활을 한다. 우리가 해야 할 일은, 사서에게 적절한 책을 주고, 사서에게 필요한 책의 내용을 이야기하는 일 밖에는 없다.

오늘은 lucene을 소개할 것은 아니고, '도로명'에 적용할 수 있도록 작성된 예제 소스를 보도록 하겠다. '도로명'은 말도 많고 탈도 많은 2014년 1월부터 시행되는 땅의 번호가 아닌 도로가 중심이 되는 새로운 주소 체계이다. 

#Introduction 소개#

lucene은 단언컨데, 그 자체로만으로도 가장 깔끔한 엔진이다. 이미, 대부분의 것들은 구현되어 있으므로, 우리가 할 일은 Document를 만들어 lucene이 index를 생성할 수 있도록 하는 일이다. 그리고 앞에서 생성한 index를 검색하는 일이 전부이다. 


##Projects##


* **AddressByStreet** : Document 생성 및 Index를 검색하는 등, 실제적으로 Lucene을 작동시키는 프로젝트
* AddressByStreetTest : AddressByStreet의 테스트 프로젝트 
* AddressBook : AddressByStreet는 단순히 Library이므로, 이를 보여줄 간단한 WPF View 프로젝트 

##Class##


#### Data Object

* RawAddress : Data Objects, juso.or.kr에서 배포하는 형태의 주소 object
* Address : Data Object, lucene에서 사용되는 주소 object
* AddressReader : Index를 읽어 들여서, 처리하는 

#### Related Lucene

* AddressSearch : Lucene.Net.Index.IndexReader를 이용해서, 검색을 하는 클래스
  * CreateLuceneIndex(Address, IndexWriter) : Index를 생성하는 메소드
  * Search(string searchQuery, string searchField) : 생성된 Index를 바탕으로 검색을 하는 메소드  

#### Library

* Enviornment : 사용되는 옵션에 대한 전역 변수
* Preference : 저장할 필요가 있는 사용자 설정 값의 Container, '.lucene'이라는 이름으로 저장된다.
 
##Requirement

* Lucene.Net : nuget으로 쉽게 설치할 수 있다.


##Enviornment Variables

* Enviornment.DataDir : Document의 Source가 되는 폴더, 즉 여기에 도로명 주소의 TXT파일 혹은 ZIP파일이 위치해야 한다. 디폴트 값은 '<실행파일의 폴더>\LuceneData'이다. 
* Enviornment.IndexDir : Document를 바탕으로 만들어진 Index가 실질적으로 저장되는 폴더, Lucene에 의해 관리되므로 실질적으로 신경쓸 내용은 없다. 디폴트 값은 '<실행파일의 폴더>\LuceneIndex'이다.
* Enviornment.AllowLeadingWildcard : '*'에 의한 검색의 지원 여부

##Explanation

* Index를 생성하는 부분

        public void AddToLuceneIndex(IEnumerable<Address> dataToIndex)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var element in dataToIndex)
                    CreateLuceneIndex(element, writer);

                analyzer.Close();
                writer.Optimize();
                writer.Dispose();
            }
            IsIndexed = true;
        }

* Search를 수행하는 부분

        public IEnumerable<Address> Search(string searchQuery, string searchField)
        {
            if (!IsIndexed) throw new FileNotFoundException("Index is not exists.");
            using (var searcher = new IndexSearcher(Directory, false))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                QueryParser parser;
                if (!string.IsNullOrEmpty(searchField))
                {
                    parser = new QueryParser(Version.LUCENE_30, searchField, analyzer);
                }
                else
                {
                    parser = new MultiFieldQueryParser(Version.LUCENE_30,
                                                       new[] {"Street", "LandLot", "Post"},
                                                       analyzer);
                }
                parser.AllowLeadingWildcard = Environment.AllowLeadingWildcard;
                var query = ParseQuery(searchQuery, parser);
                var hits = searcher.Search(query, null, Environment.MaxResult, Sort.RELEVANCE).ScoreDocs;
                var results = MapLuceneToDataList(hits, searcher);
                analyzer.Close();
                searcher.Dispose();
                return results;
            }
        }
 
 
 

# Reference 참고

* http://www.codeproject.com/Articles/609980/Small-Lucene-NET-Demo-App
* http://www.dotlucene.net/30648/lucene-net-api-search-demo
 