﻿using AddressByStreet;
using AddressByStreet.Lib;
using AddressByStreet.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AddressByStreetTest
{


    /// <summary>
    ///이 클래스는 ReaderTest에 대한 테스트 클래스로서
    ///ReaderTest 단위 테스트를 모두 포함합니다.
    ///</summary>
    [TestClass()]
    public class LibTest
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
        public void HashTest()
        {
            string file = @"C:\Users\zestime\Downloads\Lucene_VS2012_Demo_App\Lucene\SimpleLuceneSearch\bin\Debug\Lucene\대표지번_세종특별자치시.txt";
            Console.WriteLine(FileManager.GetHashString(file));
        }

    }
}
