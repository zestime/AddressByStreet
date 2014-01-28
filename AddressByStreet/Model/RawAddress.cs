using System.Linq;
using System.Text.RegularExpressions;

namespace AddressByStreet.Model
{
    public class RawAddress
    {
        // TODO - Attributes for Lucene

        // http://en.wikipedia.org/wiki/List_of_cities_in_South_Korea
        // 아래의 영문 구획 이름은 위의 페이지를 참조

        public string Code { get; set; }

        /// <summary>
        /// Province (도 道 do)
        /// Special autonomous province(특별자치도 特別自治道teukbyeol-jachi-do)
        /// Special city(특별시 特別市 teukbyeol-si)
        /// Metropolitan city(광역시 廣域市 gwangyeok-si)
        /// Special autonomous city(특별자치시 特別自治市teukbyeol-jachi-si)
        /// </summary>
        public string ProvincialName { get; set; }

        /// <summary>
        /// City(시 市 si)
        /// County(군 郡 gun)
        /// District(구 區 gu)
        /// </summary>
        public string MunicipalName { get; set; }

        /// <summary>
        /// District(구 區 gu)
        /// Town(읍 邑 eup)
        /// Township(면 面 myeon)
        /// Neighbourhood(동 洞 dong)
        /// </summary>
        public string SubmunicipalName { get; set; }

        /// <summary>
        /// Village(리 里 ri)
        /// </summary>
        public string VillageName { get; set; }
        
        /// <summary>
        /// 지번 본번
        /// </summary>
        public string LandLotMainNumber { get; set; }

        /// <summary>
        /// 지번 부번
        /// </summary>
        public string LandLotSubNumber { get; set; }

        /// <summary>
        /// 도로명
        /// </summary>
        public string StreetName { get; set; }

        /// <summary>
        /// 건물 본번
        /// </summary>
        public string BuildingMainNumber { get; set; }

        /// <summary>
        /// 건물 부번
        /// </summary>
        public string BuildingSubNumber { get; set; }

        /// <summary>
        /// 건축물대장 건물명
        /// </summary>
        public string RegisteredBuildingName { get; set; }

        /// <summary>
        /// 시군구용 건물명
        /// </summary>
        public string AdministrativeBuildingName { get; set; }

        /// <summary>
        /// 행정 구역상 이름
        /// </summary>
        public string AdministrativeSubmunicipalName { get; set; }

        /// <summary>
        /// 우편번호
        /// </summary>
        public string PostNumber { get; set; }
    }
}