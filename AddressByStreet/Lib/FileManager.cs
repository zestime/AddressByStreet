using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using AddressByStreet.Model;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace AddressByStreet.Lib
{
    public class FileManager
    {
        public static IEnumerable<string> Load(string name)
        {
            string[] files;
            
            if (Directory.Exists(name))
            {
                files = Directory.GetFiles(name);
            }
            else if (File.Exists(name))
            {
                files = new[]{name};
            }
            else
            {
                throw new FileNotFoundException();
            }

            return files.SelectMany(GetExtractedFiles);
        }

        public static IEnumerable<string> GetExtractedFiles(string name)
        {
            IEnumerable<string> list;

            if (name.EndsWith("zip", StringComparison.InvariantCultureIgnoreCase) && !Environment.Preferences.ContainIndex(name))
            {
                Environment.Preferences.AddIndex(name);
                list = Unzip(name);
            }
            else
            {
                list = Enumerable.Repeat(name, 1);
            }
            
            var output = list.Where(s => !Environment.Preferences.IndexList.ContainsKey(GetHashString(s))).ToArray();
            list.ToList().ForEach(Environment.Preferences.AddIndex);

            return output;
        }

        public static void TempClear()
        {
            if (Directory.Exists(Environment.TempDir))
                Directory.Delete(Environment.TempDir, true);
        }

        public static string GetHashString(string filename)
        {
            var hash = GetHash(filename);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public static byte[] GetHash(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    return md5.ComputeHash(stream);
                }
            }
        }

        /// <summary>
        /// 
        /// https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples
        /// </summary>
        /// <param name="archivefile">a name of Compressed file</param>
        /// <returns></returns>
        public static IEnumerable<string> Unzip(string archivefile)
        {
            ZipFile zf = null;
            IList<string> extractedfiles;

            try
            {
                FileStream fs = File.OpenRead(archivefile);
                zf = new ZipFile(fs);
                extractedfiles = new List<string>();

                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    string fullZipToPath = Path.Combine(Environment.TempDir, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                    extractedfiles.Add(fullZipToPath);
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }

            return extractedfiles;
        }
    }
}