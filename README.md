
#Background ���#

lucene�� hadoop�� �ƹ��� ���� �����̶�� ����� 2005�⿡ ���� ���� �ҽ� �˻� �����̴�. �׳� ���� ���۰����Ŷ�� �����ϸ� �ȴ�. ���𰡸� ã�� ��, ���� �� �� �ֵ��� ������ ���� �����̴�. �̸��׸�, �������� �����з����� ���� �������ִ� �缭�� ���� ��Ȱ�� �Ѵ�. �츮�� �ؾ� �� ����, �缭���� ������ å�� �ְ�, �缭���� �ʿ��� å�� ������ �̾߱��ϴ� �� �ۿ��� ����.

������ lucene�� �Ұ��� ���� �ƴϰ�, '���θ�'�� ������ �� �ֵ��� �ۼ��� ���� �ҽ��� ������ �ϰڴ�. '���θ�'�� ���� ���� Ż�� ���� 2014�� 1������ ����Ǵ� ���� ��ȣ�� �ƴ� ���ΰ� �߽��� �Ǵ� ���ο� �ּ� ü���̴�. 

#Introduction �Ұ�#

lucene�� �ܾ�����, �� ��ü�θ����ε� ���� ����� �����̴�. �̹�, ��κ��� �͵��� �����Ǿ� �����Ƿ�, �츮�� �� ���� Document�� ����� lucene�� index�� ������ �� �ֵ��� �ϴ� ���̴�. �׸��� �տ��� ������ index�� �˻��ϴ� ���� �����̴�. 


##Projects##


* **AddressByStreet** : Document ���� �� Index�� �˻��ϴ� ��, ���������� Lucene�� �۵���Ű�� ������Ʈ
* AddressByStreetTest : AddressByStreet�� �׽�Ʈ ������Ʈ 
* AddressBook : AddressByStreet�� �ܼ��� Library�̹Ƿ�, �̸� ������ ������ WPF View ������Ʈ 

##Class##


#### Data Object

* RawAddress : Data Objects, juso.or.kr���� �����ϴ� ������ �ּ� object
* Address : Data Object, lucene���� ���Ǵ� �ּ� object
* AddressReader : Index�� �о� �鿩��, ó���ϴ� 

#### Related Lucene

* AddressSearch : Lucene.Net.Index.IndexReader�� �̿��ؼ�, �˻��� �ϴ� Ŭ����
  * CreateLuceneIndex(Address, IndexWriter) : Index�� �����ϴ� �޼ҵ�
  * Search(string searchQuery, string searchField) : ������ Index�� �������� �˻��� �ϴ� �޼ҵ�  

#### Library

* Enviornment : ���Ǵ� �ɼǿ� ���� ���� ����
* Preference : ������ �ʿ䰡 �ִ� ����� ���� ���� Container, '.lucene'�̶�� �̸����� ����ȴ�.
 
##Requirement

* Lucene.Net : nuget���� ���� ��ġ�� �� �ִ�.


##Enviornment Variables

* Enviornment.DataDir : Document�� Source�� �Ǵ� ����, �� ���⿡ ���θ� �ּ��� TXT���� Ȥ�� ZIP������ ��ġ�ؾ� �Ѵ�. ����Ʈ ���� '<���������� ����>\LuceneData'�̴�. 
* Enviornment.IndexDir : Document�� �������� ������� Index�� ���������� ����Ǵ� ����, Lucene�� ���� �����ǹǷ� ���������� �Ű澵 ������ ����. ����Ʈ ���� '<���������� ����>\LuceneIndex'�̴�.
* Enviornment.AllowLeadingWildcard : '*'�� ���� �˻��� ���� ����

##Explanation

* Index�� �����ϴ� �κ�

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

* Search�� �����ϴ� �κ�

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
 
 
 

# Reference ����

* http://www.codeproject.com/Articles/609980/Small-Lucene-NET-Demo-App
* http://www.dotlucene.net/30648/lucene-net-api-search-demo
 