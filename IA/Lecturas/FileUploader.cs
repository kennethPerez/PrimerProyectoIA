using System;
using System.IO;
using System.Web;
using Spire.Doc;


namespace IA.Lecturas
{
    public class FileUploader
    {
        public string cargarArchivos(HttpFileCollection archivos)
        {
            string txtDirectorio = "C:/IA";
            
            for (int i = 0; i < archivos.Count; i++)
            {
                HttpPostedFile archivoActual = archivos[i];
                try
                {
                    if (archivoActual.ContentLength > 0)
                    {

                        String nombre = Path.GetFileName(archivoActual.FileName);
                        archivoActual.SaveAs(txtDirectorio + "\\" + nombre);

                        String[] result = nombre.Split('.');
                        if (result[1].Equals("txt"))
                        {
                            System.IO.StreamReader myFile = new System.IO.StreamReader(txtDirectorio + "\\" + Path.GetFileName(archivoActual.FileName));
                            string myString = myFile.ReadToEnd();
                            myFile.Close();
                            return myString;
                        }
                        if (result[1].Equals("docx"))
                        {
                            Document document = new Document();
                            document.LoadFromFile(txtDirectorio + "\\" + Path.GetFileName(archivoActual.FileName));
                            document.SaveToFile(txtDirectorio + "\\" + "ToText.txt", FileFormat.Txt);
                            System.IO.StreamReader myFile = new System.IO.StreamReader(txtDirectorio + "\\" + "ToText.txt");
                            string myString = myFile.ReadToEnd();
                            myFile.Close();
                            return myString;
                        }
                    }

                    else return "Error No ha seleccionado un archivo o este esta vacio.";
                }
                catch (Exception)
                {
                    return "Error al cargar los archivos.";
                }
            }

            return "Formato de archivo no valido.";
        }
    }
}