using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ionic.Zip;

namespace IA.Lecturas
{
    public class UnzipJson
    {
        public string UnzipJsonText(string path)
        {

            string zipToUnpack = @path;
            string unpackDirectory = @"C:\IA\Comprimido";
            using (ZipFile zip1 = ZipFile.Read(zipToUnpack))
            {
                foreach (ZipEntry e in zip1)
                {
                    e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            System.IO.StreamReader myFile = new System.IO.StreamReader(unpackDirectory + "\\prueba.json");
            string myString = myFile.ReadToEnd();
            myFile.Close();
            return myString;
        }
    }
}