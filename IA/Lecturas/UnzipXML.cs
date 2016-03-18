using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Ionic.Zip;
using System.Windows.Forms;

namespace IA.Lecturas
{
    public class UnzipXML
    {
        public string UnzipXml(string path)
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


            string dir = @unpackDirectory+"\\prueba.xml";
            string resultado = "";
            XmlTextReader textReader = new XmlTextReader(dir);
            textReader.Read();
            while (textReader.Read())
            {
                textReader.MoveToElement();
                string nombre = textReader.Value;

                if (nombre.Equals(' '))
                {
                    MessageBox.Show("prueba", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                resultado = resultado + " " + nombre;
            }
            return resultado;
        }
    }
}