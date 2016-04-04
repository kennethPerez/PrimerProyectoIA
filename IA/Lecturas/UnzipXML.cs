using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Ionic.Zip;
using System.Windows.Forms;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;

namespace IA.Lecturas
{
    public class UnzipXML
    {
        public string UnzipXml(string path)
        {

            string zipToUnpack = @path;
            string unpackDirectory = @"C:\IA\Comprimido";

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
                    string decompressedFileName = unpackDirectory + "//decompressed.xml";
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

            string[] xml = Directory.GetFiles(unpackDirectory, "*.xml", SearchOption.AllDirectories);

            string resultado = "";
            foreach (string name in xml)
            {
                XmlTextReader textReader = new XmlTextReader(name);
                textReader.Read();
                while (textReader.Read())
                {
                    textReader.MoveToElement();
                    string nombre = textReader.Value;
                    resultado = resultado + " " + nombre;
                }
            }
            return resultado;
        }
    }
}