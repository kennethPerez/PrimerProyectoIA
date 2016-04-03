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
            /*System.Data.Entity.Database.SetInitializer(new IAInicializador());
            IAContext db = new IAContext();
            db.Database.Initialize(true);*/
        }

        public void idioma_click(object sender, EventArgs e) {
            if (text_area.Text != "" && text_area.Text != "Debe de ingresar algun texto para ser clasificado." &&
                text_area.Text != "Debe ingresar una url." && text_area.Text != "Debe de ingresar algun texto para ser identificado." &&
                text_area.Text != "Error no ha seleccionado un archivo o este esta vacio." && text_area.Text != "Error al cargar los archivos." && text_area.Text != "Formato de archivo no valido.")
            {
                string Idioma = languageDetector.classifier(text_area.Text);
                language.Text = "El idioma detectado es: " + Idioma;

                DataTable result = LanguageDetector.DictionaryToDatatable(languageDetector.DictText);
                DataTable origin = LanguageDetector.DictionaryToDatatable(languageDetector.GetDictionary(Idioma));

                ltResult.Text = DrawChartResult.BindChart(result, "chartResult", "Frecuencia de letras en el texto", "Letras");
                ltBase.Text = DrawChartBase.BindChart(origin, "chartBase", "Frecuencia del idiama " + Idioma, "Letras");
            }
            else
                text_area.Text = "Debe ingresar un texto para poder detectar el idioma.";
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
            
            var query = from a in db.muestra join b in db.relacion on a.muestrasID equals b.muestraID join c in db.palabras on b.palabraID equals c.palabraID where c.IDdioma == a.IDdioma select new { muestra = a.muestrasID ,categoria = a.categoria, palabra = c.palabra , frecuencia = b.frecuencia  } ;

           
            foreach (var jsonResult in query)
            {
                res respuesta = estaEnLista(data, jsonResult.muestra);
                if (respuesta.boolean)  //si ya existe en la lista
                {
                    data.ElementAt(respuesta.index).palabra.Add(new bayesPalabra(jsonResult.palabra, jsonResult.frecuencia));
                }
                else     //sino existe
                {
                    List<bayesPalabra> lista =new List<bayesPalabra>();
                    lista.Add(new bayesPalabra(jsonResult.palabra, jsonResult.frecuencia));
                    data.Add(new bayesCategoria(jsonResult.categoria, lista, jsonResult.muestra));

                }

            }
            
            List<bayesCategoria> categorias = new List<bayesCategoria>();
            var result = db.muestra.Select(m => m.categoria).Distinct();

            foreach (string categoria in result)
            {
                categorias.Add(new bayesCategoria(categoria, new List<bayesPalabra>()));
            }
                       

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

        private class res
        {
            public bool boolean;
            public int index;
            public res(bool b, int t)
            {
                this.boolean = b;
                this.index = t;
            }
        }


        private res estaEnLista(List<bayesCategoria> data,int muestraID)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data.ElementAt(i).muestraId == muestraID)
                {
                    return new res(true, i);
                }
            }
            return new res(false,0);
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