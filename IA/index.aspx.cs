using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using IA.Lecturas;
using Facebook;
using IA.DataBaseContext;
using System.Dynamic;
using System.Net;
using IA.bayes_algoritmo;
using IA.detectarIdioma;
using System.Data;

namespace IA
{
    public partial class index : System.Web.UI.Page
    {
        private IAContext db = new IAContext();

        UnzipXML UnzipXml = new UnzipXML();
        FileUploader FileU = new FileUploader();
        FileHtmlUpload FileH = new FileHtmlUpload();
        UrlUploader GetUrl = new UrlUploader();
        getCarpeta GetFolder = new getCarpeta();
        UnzipJson GetJson = new UnzipJson();
        Twitter GetTweets = new Twitter();

        LanguageDetector languageDetector = new LanguageDetector();
        DrawChart DrawChartResult = new DrawChart();
        DrawChart DrawChartBase = new DrawChart();


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void idioma_click(object sender, EventArgs e) {

            string Idioma = languageDetector.classifier(text_area.Text);
            language.Text = "El idioma detectado es: "+Idioma;

            DataTable result = LanguageDetector.DictionaryToDatatable(languageDetector.DictText);
            DataTable origin = LanguageDetector.DictionaryToDatatable(languageDetector.GetDictionary(Idioma));

            ltResult.Text = DrawChartResult.BindChart(result, "chartResult", "Frecuencia de letras en el texto", "Letras");
            ltBase.Text = DrawChartBase.BindChart(origin, "chartBase", "Frecuencia del idiama "+Idioma, "Letras");
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
        protected void categorizar(object sender, EventArgs e)
        {

            List<bayesCategoria> data = new List<bayesCategoria>();

            List<bayesPalabra> t1 = new List<bayesPalabra>();
            List<bayesPalabra> t2 = new List<bayesPalabra>();

            List<bayesPalabra> r1 = new List<bayesPalabra>();
            List<bayesPalabra> r2 = new List<bayesPalabra>();

            List<bayesPalabra> s1 = new List<bayesPalabra>();
            List<bayesPalabra> s2 = new List<bayesPalabra>();

            t1.Add(new bayesPalabra("tecnologia", 2));
            t1.Add(new bayesPalabra("computacion", 2));
            t1.Add(new bayesPalabra("biblia", 0));
            t1.Add(new bayesPalabra("religion", 1));
            t1.Add(new bayesPalabra("asesino", 1));
            t1.Add(new bayesPalabra("arma", 0));
            t1.Add(new bayesPalabra("celular", 2));
            t1.Add(new bayesPalabra("muerte", 1));

            t2.Add(new bayesPalabra("tecnologia", 3));
            t2.Add(new bayesPalabra("computacion", 3));
            t2.Add(new bayesPalabra("biblia", 1));
            t2.Add(new bayesPalabra("religion", 0));
            t2.Add(new bayesPalabra("asesino", 0));
            t2.Add(new bayesPalabra("arma", 1));
            t2.Add(new bayesPalabra("celular", 3));
            t2.Add(new bayesPalabra("muerte", 0));

            data.Add(new bayesCategoria("Tecnologia", t1));
            data.Add(new bayesCategoria("Tecnologia", t2));


            r1.Add(new bayesPalabra("tecnologia", 1));
            r1.Add(new bayesPalabra("computacion", 1));
            r1.Add(new bayesPalabra("biblia", 2));
            r1.Add(new bayesPalabra("religion", 2));
            r1.Add(new bayesPalabra("asesino", 1));
            r1.Add(new bayesPalabra("arma", 1));
            r1.Add(new bayesPalabra("celular", 1));
            r1.Add(new bayesPalabra("muerte", 2));

            r2.Add(new bayesPalabra("tecnologia", 0));
            r2.Add(new bayesPalabra("computacion", 0));
            r2.Add(new bayesPalabra("biblia", 3));
            r2.Add(new bayesPalabra("religion", 3));
            r2.Add(new bayesPalabra("asesino", 0));
            r2.Add(new bayesPalabra("arma", 0));
            r2.Add(new bayesPalabra("celular", 0));
            r2.Add(new bayesPalabra("muerte", 1));

            data.Add(new bayesCategoria("religion", r1));
            data.Add(new bayesCategoria("religion", r2));

            s1.Add(new bayesPalabra("tecnologia", 1));
            s1.Add(new bayesPalabra("computacion", 1));
            s1.Add(new bayesPalabra("biblia", 1));
            s1.Add(new bayesPalabra("religion", 1));
            s1.Add(new bayesPalabra("asesino", 2));
            s1.Add(new bayesPalabra("arma", 2));
            s1.Add(new bayesPalabra("celular", 1));
            s1.Add(new bayesPalabra("muerte", 2));

            s2.Add(new bayesPalabra("tecnologia", 0));
            s2.Add(new bayesPalabra("computacion", 0));
            s2.Add(new bayesPalabra("biblia", 0));
            s2.Add(new bayesPalabra("religion", 0));
            s2.Add(new bayesPalabra("asesino", 3));
            s2.Add(new bayesPalabra("arma", 1));
            s2.Add(new bayesPalabra("celular", 0));
            s2.Add(new bayesPalabra("muerte", 3));

            data.Add(new bayesCategoria("sucesos", s1));
            data.Add(new bayesCategoria("sucesos", s2));


            List<bayesCategoria> categorias = new List<bayesCategoria>();
            categorias.Add(new bayesCategoria("Tecnologia", new List<bayesPalabra>()));
            categorias.Add(new bayesCategoria("religion", new List<bayesPalabra>()));
            categorias.Add(new bayesCategoria("sucesos", new List<bayesPalabra>()));


            string texto = "La tecnologia tecnologia tecnologia tecnologia utilizando un celular en la religion";

            Console.WriteLine("Texto: " + texto.ToLower() + "\n");

            Respuesta Finalresult = new NaiveBayes(data, categorias, texto.ToLower()).classifier();
            string concat="";
            foreach (ResultNaiveBayes r in Finalresult.listaDeProbabilidadesFinales)
            {
                concat+=r.categoria + " = " + r.probabilidad * 100 + "%\n";
            }
            text_area.Text = concat;

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