using System.Linq;
using System.Text.RegularExpressions;
using AddressByStreet;
using AddressByStreet.Lib;
using AddressByStreet.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AddressByStreetTest
{
    
    
    /// <summary>
    ///이 클래스는 ReaderTest에 대한 테스트 클래스로서
    ///ReaderTest 단위 테스트를 모두 포함합니다.
    ///</summary>
    [TestClass()]
    public class LuceneTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///현재 테스트 실행에 대한 정보 및 기능을
        ///제공하는 테스트 컨텍스트를 가져오거나 설정합니다.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 추가 테스트 특성
        // 
        //테스트를 작성할 때 다음 추가 특성을 사용할 수 있습니다.
        //
        //ClassInitialize를 사용하여 클래스의 첫 번째 테스트를 실행하기 전에 코드를 실행합니다.
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //ClassCleanup을 사용하여 클래스의 테스트를 모두 실행한 후에 코드를 실행합니다.
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //TestInitialize를 사용하여 각 테스트를 실행하기 전에 코드를 실행합니다.
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //TestCleanup을 사용하여 각 테스트를 실행한 후에 코드를 실행합니다.
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod]
        public void CreateIndexTest()
        {
            string file = @"C:\Users\zestime\Downloads\Lucene_VS2012_Demo_App\Lucene\SimpleLuceneSearch\bin\Debug\Lucene\대표지번_세종특별자치시.txt";
            Environment.BaseDir = @"C:\Temp";
            AddressReader reader = new AddressReader();
            DefaultFormatter formatter = new DefaultFormatter();
            var list = reader.Read(file).Select(formatter.FormatTo);
            AddressSearch searcher = new AddressSearch();
            searcher.AddToLuceneIndex(list);
        }
    
        [TestMethod]
        public void SearchTest()
        {
            Environment.BaseDir = @"C:\Temp";
            AddressSearch search = new AddressSearch();

            string searchQuery = "장군면";
            string searchField = "Street";

            var r = search.Search(searchQuery, searchField);
            Assert.AreEqual(2279, r.Count());

            searchQuery = "장군면 437";
            r = search.Search(searchQuery, searchField);
            Assert.AreEqual(2280, r.Count());

            r = search.Search(searchQuery);
            Assert.AreEqual(2290, r.Count());
            
            searchQuery = "251 세종국제*";
            r = search.Search(searchQuery, searchField);
            Assert.AreEqual("3611011300103600005000001", r.ToArray()[0].Code);

            searchQuery = "*국제고등*";
            r = search.Search(searchQuery);
            Assert.AreEqual(1, r.Count());
            //Assert.AreEqual("3611011300103600005000001", r.ToArray()[0].Code);
        }
    }
}
