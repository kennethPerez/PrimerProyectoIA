using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ionic.Zip;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;

namespace IA.Lecturas
{
    public class UnzipJson
    {
        public string UnzipJsonText(string path)
        {

            string zipToUnpack = @path;
            string unpackDirectory = @"C:\IA\Comprimido";

            string myString = "";

            try
            {
                using (ZipFile zip1 = ZipFile.Read(zipToUnpack))
                {
                    foreach (ZipEntry e in zip1)
                    {
                        e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                    }
                }

            }
            catch (Exception e)
            {
                FileInfo zipFileName = new FileInfo(zipToUnpack);

                using (FileStream fileToDecompressAsStream = zipFileName.OpenRead())
                {
                    string decompressedFileName = unpackDirectory + "//decompressed.json";
                    using (FileStream decompressedStream = File.Create(decompressedFileName))
                    {
                        try
                        {
                            BZip2.Decompress(fileToDecompressAsStream, decompressedStream, true);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }

            string[] json = Directory.GetFiles(unpackDirectory, "*.json", SearchOption.AllDirectories);
            StreamReader sr;

            foreach (string name in json)
            {
                sr = new StreamReader(name);
                myString += sr.ReadToEnd() + '\n';
                sr.Close();
            }

            return myString;
        }
    }
}