using AddressByStreet;
using AddressByStreet.Lib;
using AddressByStreet.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AddressByStreetTest
{
    
    
    /// <summary>
    ///이 클래스는 ReaderTest에 대한 테스트 클래스로서
    ///ReaderTest 단위 테스트를 모두 포함합니다.
    ///</summary>
    [TestClass()]
    public class AddressTest
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
        public void AddressInstanceTest()
        {
            var content = @"1111010100|서울특별시|종로구|청운동||0|1|0|111104100289|자하문로36길|0|16|14|청운벽산빌리지||1111010100100010000030777|01|1111051500|청운효자동|110030|001|||||벽산빌라|1";

            AddressReader reader = new AddressReader();
            var r = reader.Create(content);

            Assert.IsInstanceOfType(r, typeof(RawAddress));
            Assert.AreEqual("서울특별시", r.ProvincialName);
            Assert.AreEqual("종로구", r.MunicipalName);
            Assert.AreEqual("청운동", r.SubmunicipalName);
            Assert.AreEqual("", r.VillageName);
            Assert.AreEqual("자하문로36길", r.StreetName);
            Assert.AreEqual("16", r.BuildingMainNumber);
            Assert.AreEqual("14", r.BuildingSubNumber);
            Assert.AreEqual("벽산빌라", r.AdministrativeBuildingName);
            Assert.AreEqual("1", r.LandLotMainNumber);
            Assert.AreEqual("0", r.LandLotSubNumber);
            Assert.AreEqual("청운효자동", r.AdministrativeSubmunicipalName);
            Assert.AreEqual("110030", r.PostNumber);
        }

        [TestMethod]
        public void GetAddressTest()
        {
            var content = @"3611025023|세종특별자치시||조치원읍|평리|0|23|10|361104574332|수원지3길|0|14|0|평동연립||4473025023100230010037207|01|||339885|021|||||평동연립|0";

            AddressReader reader = new AddressReader();
            var r = reader.Create(content);

            DefaultFormatter formatter = new DefaultFormatter();
            var addr = formatter.FormatTo(r);
            Assert.AreEqual("4473025023100230010037207", addr.Code);
            Assert.AreEqual("세종특별자치시 조치원읍 수원지3길 14 (평동연립)", addr.Street);
            Assert.AreEqual("세종특별자치시 조치원읍 평리 23-10 (평동연립)", addr.LandLot);
            Assert.AreEqual("339-885", addr.Post);
        }

        [TestMethod]
        public void GetAddressTest2()
        {
            var content = @"3611025024|세종특별자치시||조치원읍|교리|0|129|1|361104574018|건강길|0|16|0|||4473025024101290001000001|01|||339801|021|||||세종특별자치시보건소|0";

            AddressReader reader = new AddressReader();
            var r = reader.Create(content);

            DefaultFormatter formatter = new DefaultFormatter();
            var addr = formatter.FormatTo(r);
            Assert.AreEqual("4473025024101290001000001", addr.Code);
            Assert.AreEqual("세종특별자치시 조치원읍 건강길 16 (세종특별자치시보건소)", addr.Street);
            Assert.AreEqual("세종특별자치시 조치원읍 교리 129-1 (세종특별자치시보건소)", addr.LandLot);
            Assert.AreEqual("339-801", addr.Post);
        }

        [TestMethod]
        public void GetAddressTest3()
        {
            var content = @"1153010200|서울특별시|구로구|구로동||0|718|24|115304148200|구로동로13길|0|34|4||8동|1153010200107180024024498|01|1153053000|구로제2동|152871|001|||||경원주택|1";

            AddressReader reader = new AddressReader();
            var r = reader.Create(content);

            DefaultFormatter formatter = new DefaultFormatter();
            var addr = formatter.FormatTo(r);
            Assert.AreEqual("1153010200107180024024498", addr.Code);
            Assert.AreEqual("서울특별시 구로구 구로동 구로동로13길 34-4 (구로2동 경원주택)", addr.Street);
            Assert.AreEqual("서울특별시 구로구 구로동 718-24 (구로2동 경원주택)", addr.LandLot);
            Assert.AreEqual("152-871", addr.Post);
        }
    }
}
