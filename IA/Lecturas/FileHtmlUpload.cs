using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace IA.Lecturas
{
    public class FileHtmlUpload
    {
        public string HtmlFile(string Path)
        {
            StreamReader streamReader = new StreamReader(Path);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            return text;
        }
    }
}