using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AddressByStreet.Lib;
using AddressByStreet.Model;

namespace AddressByStreet
{
    public class AddressReader : IRawDataReader<RawAddress>
    {
        /// <summary>
        /// Nomally, We may treat Korean
        /// </summary>
        public readonly Encoding DefaultEncoding = Encoding.GetEncoding("EUC-KR");
        public Dictionary<string, string> IndexedFiles { get; set; }

        public AddressReader()
        {
            IndexedFiles = new Dictionary<string, string>();
        }

        public AddressReader(Encoding encoding) : this()
        {
            DefaultEncoding = encoding;
        }

        public IEnumerable<RawAddress> Read(IEnumerable<string> filename)
        {
            return filename.SelectMany(Read);
        }

        public IEnumerable<RawAddress> Read(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException();

            string hash = FileManager.GetHashString(filename);
            if (IndexedFiles.ContainsKey(hash))
                throw new ArgumentException("The file already indexed.", filename);

            IndexedFiles.Add(hash, filename);

            var list = File.ReadAllLines(filename, DefaultEncoding);
            return list.Select(Create);
        }

        private RawAddress MapStringArrayToRawAddress(string[] arr)
        {                                       
            return new RawAddress
            {
                ProvincialName = arr[1],
                MunicipalName = arr[2],
                SubmunicipalName = arr[3],
                VillageName = arr[4],
                LandLotMainNumber = arr[6],
                LandLotSubNumber = arr[7],
                StreetName = arr[9],
                BuildingMainNumber = arr[11],
                BuildingSubNumber = arr[12],
                RegisteredBuildingName = arr[13],
                Code = arr[15],
                AdministrativeSubmunicipalName = arr[18],
                PostNumber = arr[19],
                AdministrativeBuildingName = arr[25],
            };
        }

        // TODO - Create RawFormatter then, delete this
        public RawAddress Create(string content)
        {
            if ( string.IsNullOrEmpty(content))
                throw new ArgumentException("It's null or empty", content);

            string[] items = content.Split('|');
            return MapStringArrayToRawAddress(items);
        }
    }
}