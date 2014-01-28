using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AddressByStreet.Lib
{
    public class Preferences
    {
        private PrefsData _preData;
        public PrefsData PreData
        {
            get { return _preData; }
            set 
            { 
                _preData = value; 
                
                if (_preData.IndexArray == null)
                    _preData.IndexArray = new ArrayList();

                if (_preData.IndexArray.Count > _preData.MaxIndexedCount)
                    _preData.IndexArray.RemoveRange(_preData.MaxIndexedCount,
                                                     _preData.IndexArray.Count - _preData.MaxIndexedCount);

                LoadIndexList(_preData.IndexArray, IndexList);
            }
        }

        private void LoadIndexList(ArrayList list, Dictionary<string, string> dict)
        {
            dict.Clear();
            foreach (var i in list)
            {
                string t = i.ToString();
                var p = t.Split(Environment.SplitCharacter);
                dict.Add(p[0], p[1]);
            }
        }

        private void ApplyIndexList()
        {
            PreData.IndexArray.Clear();
            foreach (var p in IndexList)
            {
                PreData.IndexArray.Add(string.Concat(p.Key, Environment.SplitCharacter, p.Value));
            }
        }

        public Dictionary<string, string> IndexList { get; set; }

        public Preferences()
        {
            IndexList = new Dictionary<string, string>();
            PreData = new PrefsData();

            Load(Environment.PreferenceFile);
        }

        public void Load(string filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PrefsData));
                using (var stream = new StreamReader(filename))
                {
                    PreData = (PrefsData) serializer.Deserialize(stream);
                }
            }
            catch
            {
            }
        }

        public void Save()
        {
            ApplyIndexList();
            XmlSerializer serializer = new XmlSerializer(typeof(PrefsData));
            serializer.Serialize(new StreamWriter(Environment.PreferenceFile), PreData);
        }

        public void AddIndex(string filename)
        {
            string md5 = FileManager.GetHashString(filename);
            if (!IndexList.ContainsKey(md5))
                IndexList.Add(md5, filename);
        }

        public bool ContainIndex(string filename)
        {
            string md5 = FileManager.GetHashString(filename);
            return IndexList.ContainsKey(md5);
        }
    }
}