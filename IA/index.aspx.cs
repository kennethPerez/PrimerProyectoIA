using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using IA.Lecturas;
using Facebook;

namespace IA
{
    public partial class index : System.Web.UI.Page
    {
        UnzipXML UnzipXml = new UnzipXML();
        FileUploader FileU = new FileUploader();
        FileHtmlUpload FileH = new FileHtmlUpload();
        UrlUploader GetUrl = new UrlUploader();
        getCarpeta GetFolder = new getCarpeta();
        UnzipJson GetJson = new UnzipJson();
        Twitter GetTweets = new Twitter();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CargarArchivo_Click(object sender, EventArgs e)
        {
            HttpFileCollection archivos = Request.Files;

            text_area.Text = FileU.cargarArchivos(archivos);
        }

        protected void HtmlFile_Click(object sender, EventArgs e)
        {
            string txtDirectorio = "C:/IA/";

            if (FileUpload.HasFile)
            {
                try
                {
                    string filename = Path.GetFileName(FileUpload.FileName);
                    Console.Write(filename);
                    FileUpload.SaveAs(txtDirectorio + filename);
                    text_area.Text = FileH.HtmlFile(txtDirectorio + "/" + filename);
                }
                catch (Exception ex)
                {
                    text_area.Text = "El archivo no se cargo correctamente. El error va dirigido a: " + ex.Message;
                }
            }
            else text_area.Text = "Html no cargado, check ruta.";

        }

        protected void ParseURL_Click(object sender, EventArgs e)
        {
            if (text_area.Text != "" && text_area.Text != "Debe de ingresar algun texto para ser clasificado." &&
                text_area.Text != "Debe ingresar una url." && text_area.Text != "Debe de ingresar algun texto para ser identificado." &&
                text_area.Text != "Error no ha seleccionado un archivo o este esta vacio." && text_area.Text != "Error al cargar los archivos." && text_area.Text != "Formato de archivo no valido.")
            {
                string[] array = GetUrl.ParserUrl(text_area.Text).Split(new string[] { "/*<![", "<div>" }, StringSplitOptions.None);

                text_area.Text = array[0];
            }
            else text_area.Text = "Debe ingresar una url.";
        }

        protected void Json_Click(object sender, EventArgs e)
        {
            
            if (text_area.Text != "" && text_area.Text != "Debe de ingresar algun texto para ser clasificado." &&
                text_area.Text != "Debe ingresar una url." && text_area.Text != "Debe de ingresar algun texto para ser identificado." &&
                text_area.Text != "Error no ha seleccionado un archivo o este esta vacio." && text_area.Text != "Error al cargar los archivos." && text_area.Text != "Formato de archivo no valido.")
            {
                text_area.Text = GetJson.UnzipJsonText(text_area.Text); 
            }
        }
        protected void Xml_Click(object sender, EventArgs e)
        {

            if (text_area.Text != "" && text_area.Text != "Debe de ingresar algun texto para ser clasificado." &&
                text_area.Text != "Debe ingresar una url." && text_area.Text != "Debe de ingresar algun texto para ser identificado." &&
                text_area.Text != "Error no ha seleccionado un archivo o este esta vacio." && text_area.Text != "Error al cargar los archivos." && text_area.Text != "Formato de archivo no valido.")
            {
                text_area.Text = UnzipXml.UnzipXml(text_area.Text);
            }
        }
        
        protected void carpeta_click(object sender, EventArgs e)
        {
            if (text_area.Text != "" && text_area.Text != "Debe de ingresar algun texto para ser clasificado." &&
                text_area.Text != "Debe ingresar una url." && text_area.Text != "Debe de ingresar algun texto para ser identificado." &&
                text_area.Text != "Error no ha seleccionado un archivo o este esta vacio." && text_area.Text != "Error al cargar los archivos." && text_area.Text != "Formato de archivo no valido.")
            {
                text_area.Text = GetFolder.GetCarpeta(text_area.Text);
            }
        }

        protected void twitter_click(object sender, EventArgs e)
        {
            if (text_area.Text != "" && text_area.Text != "Debe de ingresar algun texto para ser clasificado." &&
                text_area.Text != "Debe ingresar una url." && text_area.Text != "Debe de ingresar algun texto para ser identificado." &&
                text_area.Text != "Error no ha seleccionado un archivo o este esta vacio." && text_area.Text != "Error al cargar los archivos." && text_area.Text != "Formato de archivo no valido.")
            {
                text_area.Text = GetTweets.getTweet(text_area.Text.Split(' ')[0], Int32.Parse(text_area.Text.Split(' ')[1]));
            }
        }
    }
}