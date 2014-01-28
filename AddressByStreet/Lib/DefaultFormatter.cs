using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AddressByStreet.Model;

namespace AddressByStreet.Lib
{
    public class DefaultFormatter : IModelFormatter<RawAddress, Address>
    {
        public const string NumberSeperate = "-";
        public const string ZeroString = "0";

        public Address FormatTo(RawAddress raw)
        {
            return new Address
                       {
                           Code = raw.Code,
                           Street = GetStreetAddress(raw),
                           LandLot = GetLandLotAddress(raw),
                           Post = GetPost(raw.PostNumber)
                       };
        }

        private void AddItem(StringBuilder builder, string s)
        {
            if (!string.IsNullOrEmpty(s.Trim()))
            {
                if (builder.Length > 0) builder.Append(" ");
                builder.Append(s);
            }
        }

        private string GetStreetAddress(RawAddress a)
        {
            var addr = new StringBuilder();

            AddItem(addr, a.ProvincialName);
            AddItem(addr, a.MunicipalName);
            AddItem(addr, a.SubmunicipalName);
            AddItem(addr, a.StreetName);

            var buildingNumber = a.BuildingMainNumber;
            if (!a.BuildingSubNumber.Equals(ZeroString))
                buildingNumber += NumberSeperate + a.BuildingSubNumber;

            AddItem(addr, buildingNumber);

            return addr + GetBuildingName(a);
        }

        private string GetLandLotAddress(RawAddress a)
        {
            var addr = new StringBuilder();

            addr.Append(a.ProvincialName);
            AddItem(addr, a.MunicipalName);
            AddItem(addr, a.SubmunicipalName);
            AddItem(addr, a.VillageName);

            string landLot = a.LandLotMainNumber;
            
            if (!a.LandLotSubNumber.Equals(ZeroString))
                landLot += NumberSeperate + a.LandLotSubNumber;

            AddItem(addr, landLot);

            return addr + GetBuildingName(a);
        }

        private string GetBuildingName(RawAddress a)
        {
            var buildings = new StringBuilder();

            string subMunicipal = Regex.IsMatch(a.AdministrativeSubmunicipalName, @"제\d동")
                ? Regex.Replace(a.AdministrativeSubmunicipalName, @"제(\d동)", m => m.Groups[1].Value)
                : a.AdministrativeSubmunicipalName;

            AddItem(buildings, subMunicipal);
            AddItem(buildings, a.AdministrativeBuildingName);

            if (!a.AdministrativeBuildingName.Equals(a.RegisteredBuildingName))
                AddItem(buildings, a.RegisteredBuildingName);
    
            return buildings.Length > 0 ? string.Format(" ({0})", buildings) : string.Empty;
        }

        private string GetPost(string post)
        {
            return post.Substring(0, 3) + NumberSeperate + post.Substring(3);
        }
    }
}