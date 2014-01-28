using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace AddressByStreet.Lib
{
    public static class Environment
    {
        public const string IndexDirName = "LuceneIndex";
        public const string DataDirName = "LuceneData";
        public const string TempDirName = "LuceneTemp";

        private const string PreferenceFileName = ".lucene";

        public const char SplitCharacter = '|';
        public const int MaxResult = 1000;

        internal static readonly string PreferenceFile = Path.Combine(BaseDir, PreferenceFileName);

        private static string _baseDir;
        public static string BaseDir
        {
            get
            {
                if (String.IsNullOrEmpty(_baseDir))
                    _baseDir =  Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                return _baseDir;
            }
            set { _baseDir = value; }
        }

        private static string _indexDir;
        public static string IndexDir
        {
            get
            {
                if (String.IsNullOrEmpty(_indexDir))
                    _indexDir = Path.Combine(BaseDir, IndexDirName);
                return _indexDir;
            }
            set { _indexDir = value; }
        }

        private static string _dataDir;
        public static string DataDir
        {
            get
            {
                if (String.IsNullOrEmpty(_dataDir))
                    _dataDir = Path.Combine(BaseDir, DataDirName);
                return _dataDir;
            }
            set { _dataDir = value; }
        }

        private static string _tempDir;
        public static string TempDir
        {
            get
            {
                if (String.IsNullOrEmpty(_tempDir))
                    _tempDir = Path.Combine(BaseDir, TempDirName);
                return _tempDir;
            }
            set { _tempDir = value; }
        }

        private static Preferences _preferences;
        public static Preferences Preferences
        {
            get { return _preferences ?? (_preferences = new Preferences()); }
        }

        public static bool IndecExceptionIfNotExist { get; set; }

        public static bool AllowLeadingWildcard { get; set; }


        // TODO - Managed by config file <.AddressByStreet>
    }
}