using System.Collections;
using System.Linq;
using AddressByStreet;
using AddressByStreet.Lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AddressByStreet.Model;
using System.Collections.Generic;
using Environment = AddressByStreet.Lib.Environment;

namespace AddressByStreetTest
{
    
    
    /// <summary>
    ///이 클래스는 AddressReaderTest에 대한 테스트 클래스로서
    ///AddressReaderTest 단위 테스트를 모두 포함합니다.
    ///</summary>
    [TestClass()]
    public class AddressReaderTest
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


        /// <summary>
        ///Read 테스트
        ///</summary>
        [TestMethod()]
        public void ReadTest()
        {
            Environment.BaseDir = @"C:\Temp";
            AddressReader reader = new AddressReader();
            var files = FileManager.Load(Environment.DataDir);
            var rows = reader.Read(files);
            Assert.AreEqual(120166, rows.Count());
            foreach (KeyValuePair<string, string> p in reader.IndexedFiles)
            {
                Console.WriteLine("{0} - {1}", p.Key, p.Value);
            }
            

        }
    }
}
