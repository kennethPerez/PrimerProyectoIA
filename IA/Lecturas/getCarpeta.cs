using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Spire.Doc;

namespace IA.Lecturas
{
    public class getCarpeta
    {
        public string GetCarpeta(string PathURL)
        {
            string Result = "";
            string txtDirectorio = "C:/IA/";

            try
            {
                string[] txt = Directory.GetFiles(@PathURL, "*.txt", SearchOption.AllDirectories);
                StreamReader sr;
                
                foreach (string name in txt)
                {
                    sr = new StreamReader(name);
                    Result += sr.ReadToEnd() + '\n';
                    sr.Close();
                }
                Result += "\n";
                
                string[] doc = Directory.GetFiles(@PathURL, "*.docx", SearchOption.AllDirectories);

                foreach (string name in doc)
                {
                    Document document = new Document();
                    document.LoadFromFile(name);
                    document.SaveToFile(txtDirectorio + "\\" + "ToText.txt", FileFormat.Txt);

                    StreamReader std = new StreamReader(txtDirectorio + "\\" + "ToText.txt");

                    Result += std.ReadToEnd() + '\n';
                    std.Close();
                }
                    
                Result += "\n";
               string[] html = Directory.GetFiles(@PathURL, "*.html", SearchOption.AllDirectories);

               foreach (string name in html)
                {
                    StreamReader streamReader = new StreamReader(name);
                    string text = streamReader.ReadToEnd();
                    Result += text + '\n';
                    streamReader.Close();

                }
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Result += UAEx.Message;
            }
            catch (PathTooLongException PathEx)
            {
                Result += PathEx.Message;
            }

            return Result;
        }

    }
}