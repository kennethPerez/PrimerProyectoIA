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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IA
{
    public class ResultTweets
    {
        public string name;
        public int idIdioma;
        public string texto;
        public ResultTweets(string name, int idioma, string texto)
        {
            this.idIdioma = idioma;
            this.name = name;
            this.texto = texto;
        }
    }
    public partial class index : System.Web.UI.Page
    {
        private IAContext db = new IAContext();
        IA.aprender.Aprender aprendizaje = new IA.aprender.Aprender();
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

            //aprendizaje.aprendaUrls();

            /*System.Data.Entity.Database.SetInitializer(new IAInicializador());
            IAContext db = new IAContext();
            db.Database.Initialize(true);*/
        }

        public void idioma_click(object sender, EventArgs e) {
            
            if (text_area.Text != "" && text_area.Text != "Debe de ingresar algun texto para ser clasificado." &&
                    text_area.Text != "Debe ingresar una url." && text_area.Text != "Debe de ingresar algun texto para ser identificado." &&
                    text_area.Text != "Error no ha seleccionado un archivo o este esta vacio." && text_area.Text != "Error al cargar los archivos." && text_area.Text != "Formato de archivo no valido.")
            {

                

                if (IA.aprender.Aprender.vienenPosts) {
                    IA.aprender.Aprender.vienenPosts = false;
                    IA.aprender.Aprender.vienenTweets = false;

                    //-----------------------------------------------------
                    IA.aprender.Aprender.vienenJson = true;

                    string[] stringSeparado = text_area.Text.Split('^');

                    foreach (string palabra in stringSeparado)
                    {
                        try
                        {
                            JObject jresult = JObject.Parse(palabra);
                            int IdIdioma = 99;
                            string Idioma = languageDetector.classifier(jresult["post"].ToString());
                            if (Idioma.Equals("Ingles"))
                            {
                                IdIdioma = 2;
                            }
                            if (Idioma.Equals("Aleman"))
                            {
                                IdIdioma = 3;
                            }
                            if (Idioma.Equals("Español"))
                            {
                                IdIdioma = 1;
                            }
                            if (Idioma.Equals("Turco"))
                            {
                                IdIdioma = 4;
                            }
                            ResultTweets x = new ResultTweets(null, IdIdioma, jresult["post"].ToString());
                            IA.aprender.Aprender.ListaDeTweets.Add(x);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        /*ANALIZAR CADA post CADA EN JSON*/
                    }
                    usuariosAnalizados.Text = "La cantidad de usuarios es: 1";
                    teewsAnalizados.Text = "La cantidad de mensajes analizados es: " + IA.aprender.Aprender.ListaDeTweets.Count.ToString();

                    DrawChartMessage drm = new DrawChartMessage();
                    DataTable table = new DataTable();
                    table.Columns.Add("idioma", typeof(string));
                    table.Columns.Add("porcentaje", typeof(double));

                    double esp = 0.0;
                    double ing = 0.0;
                    double ale = 0.0;
                    double tur = 0.0;
                    foreach (ResultTweets t in IA.aprender.Aprender.ListaDeTweets)
                    {
                        if (t.idIdioma == 1)
                            esp++;
                        if (t.idIdioma == 2)
                            ing++;
                        if (t.idIdioma == 3)
                            ale++;
                        if (t.idIdioma == 4)
                            tur++;
                    }

                    table.Rows.Add("Español", (esp / IA.aprender.Aprender.ListaDeTweets.Count) * 100);
                    table.Rows.Add("Ingles", (ing / IA.aprender.Aprender.ListaDeTweets.Count) * 100);
                    table.Rows.Add("Aleman", (ale / IA.aprender.Aprender.ListaDeTweets.Count) * 100);
                    table.Rows.Add("Turco", (tur / IA.aprender.Aprender.ListaDeTweets.Count) * 100);

                    LiteralMessage.Text = drm.BindChart(table, "chartJson", "Porcentaje de idiomas", "Idiomas");
                }

                if (IA.aprender.Aprender.vienenTweets)
                {

                    IA.aprender.Aprender.vienenTweets = false;
                    string[] stringSeparado = text_area.Text.Split('\n');

                    foreach (string palabra in stringSeparado)
                    {
                        try
                        {
                            JObject jresult = JObject.Parse(palabra);
                            int IdIdioma = 99;
                            string Idioma = languageDetector.classifier(jresult["text"].ToString());
                            if (Idioma.Equals("Ingles"))
                            {
                                IdIdioma = 2;
                            }
                            if (Idioma.Equals("Aleman"))
                            {
                                IdIdioma = 3;
                            }
                            if (Idioma.Equals("Español"))
                            {
                                IdIdioma = 1;
                            }
                            if (Idioma.Equals("Turco"))
                            {
                                IdIdioma = 4;
                            }
                            ResultTweets x = new ResultTweets(jresult["user"]["screen_name"].ToString(), IdIdioma, jresult["text"].ToString());
                            IA.aprender.Aprender.ListaDeTweets.Add(x);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                        /*ANALIZAR CADA TWEETS CADA EN JSON*/
                    }
                    usuariosAnalizados.Text ="La cantidad de usuarios es: "+ IA.aprender.Aprender.ListaDeTweets.Count.ToString();
                    teewsAnalizados.Text = "La cantidad de mensajes analizados es: " + IA.aprender.Aprender.ListaDeTweets.Count.ToString();

                    DrawChartMessage drm = new DrawChartMessage();
                    DataTable table = new DataTable();
                    table.Columns.Add("idioma", typeof(string));
                    table.Columns.Add("porcentaje", typeof(double));

                    double esp = 0.0;
                    double ing = 0.0;
                    double ale = 0.0;
                    double tur = 0.0;
                    foreach (ResultTweets t in IA.aprender.Aprender.ListaDeTweets)
                    {
                        if (t.idIdioma == 1)
                            esp++;
                        if (t.idIdioma == 2)
                            ing++;
                        if (t.idIdioma == 3)
                            ale++;
                        if (t.idIdioma == 4)
                            tur++;
                    }

                    table.Rows.Add("Español", (esp / IA.aprender.Aprender.ListaDeTweets.Count) * 100);
                    table.Rows.Add("Ingles", (ing / IA.aprender.Aprender.ListaDeTweets.Count) * 100);
                    table.Rows.Add("Aleman", (ale / IA.aprender.Aprender.ListaDeTweets.Count) * 100);
                    table.Rows.Add("Turco", (tur / IA.aprender.Aprender.ListaDeTweets.Count) * 100);

                    LiteralMessage.Text = drm.BindChart(table, "chartJson", "Porcentaje de idiomas", "Idiomas");
                }

                    string idioma = languageDetector.classifier(text_area.Text);
                    language.Text = "El idioma detectado es: " + idioma;

                    DataTable result = LanguageDetector.DictionaryToDatatable(languageDetector.DictText);
                    DataTable origin = LanguageDetector.DictionaryToDatatable(languageDetector.GetDictionary(idioma));

                    ltResult.Text = DrawChartResult.BindChart(result, "chartResult", "Frecuencia de letras en el texto", "Letras");
                    ltBase.Text = DrawChartBase.BindChart(origin, "chartBase", "Frecuencia del idioma " + idioma, "Letras");
            }            
            else
                text_area.Text = "Debe ingresar un texto para poder detectar el idioma.";
            
        }

        protected void facebook(object sender, EventArgs e)
        {
            IA.aprender.Aprender.vienenPosts = true;
            IA.aprender.Aprender.ListaDeTweets.Clear();
        }

        protected void CargarArchivo_Click(object sender, EventArgs e)
        {
            HttpFileCollection archivos = Request.Files;

            text_area.Text = FileU.cargarArchivos(archivos);

        }

        protected void aprender(object sender, EventArgs e)
        {

            text_area.Text += aprendizaje.aprender(text_area.Text, 1, "Tecnologia",true,true);

        }
        protected void aprenderMuestra(object sender, EventArgs e)
        {

            JObject jresult = JObject.Parse(text_area.Text);


            text_area.Text += aprendizaje.aprender(jresult["Texto"].ToString(),Convert.ToInt32( jresult["Idioma"].ToString()), jresult["Cat"].ToString(), false,true);

            //text_area.Text += aprendizaje.aprender(text_area.Text, 1, "Tecnologia", false);

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


        private List<bayesCategoria> crearData(int idiomaId) {

            List<bayesCategoria> data = new List<bayesCategoria>();
            var query = from a in db.muestra join b in db.relacion on a.muestrasID equals b.muestraID join c in db.palabras on b.palabraID equals c.palabraID where c.IDdioma == idiomaId select new { muestra = a.muestrasID, categoria = a.categoria, palabra = c.palabra, frecuencia = b.frecuencia };

            foreach (var jsonResult in query)
            {
                res respuesta = estaEnLista(data, jsonResult.muestra);
                if (respuesta.boolean)  //si ya existe en la lista
                {
                    data.ElementAt(respuesta.index).palabra.Add(new bayesPalabra(jsonResult.palabra, jsonResult.frecuencia));
                }
                else     //sino existe
                {
                    List<bayesPalabra> lista = new List<bayesPalabra>();
                    lista.Add(new bayesPalabra(jsonResult.palabra, jsonResult.frecuencia));
                    data.Add(new bayesCategoria(jsonResult.categoria, lista, jsonResult.muestra));
                }
            }

            return data;
        }

        protected void categorizar(object sender, EventArgs e)
        {
            // Crear las categorias
            List<bayesCategoria> categorias = new List<bayesCategoria>();
            var result = db.muestra.Select(m => m.categoria).Distinct();

            foreach (string categoria in result)
            {
                categorias.Add(new bayesCategoria(categoria, new List<bayesPalabra>()));
            }



            if (IA.aprender.Aprender.vienenJson)
            {
                IA.aprender.Aprender.vienenJson = false;
                foreach (ResultTweets t in IA.aprender.Aprender.ListaDeTweets)
                {
                    Respuesta Finalresult = new NaiveBayes(crearData(t.idIdioma), categorias, t.texto.ToLower()).classifier();
                                        
                }
            }
            else
            {
                int resultIdioma = 0;
                switch (language.Text)
                {
                    case "El idioma detectado es: Español":
                        resultIdioma = 1;
                        break;
                    case "El idioma detectado es: Ingles":
                        resultIdioma = 2;
                        break;
                    case "El idioma detectado es: Turco":
                        resultIdioma = 4;
                        break;
                    case "El idioma detectado es: Aleman":
                        resultIdioma = 3;
                        break;
                }
                

                string texto = text_area.Text;
                Respuesta Finalresult = new NaiveBayes(crearData(resultIdioma), categorias, texto.ToLower()).classifier();

                DrawChartCategories drc = new DrawChartCategories();
                DataTable table = new DataTable();
                table.Columns.Add("categoria", typeof(string));
                table.Columns.Add("porcentaje", typeof(double));

                List<resN> probFinales = new List<resN>();
                foreach (ResultNaiveBayes r in Finalresult.listaDeProbabilidadesFinales)
                {
                    table.Rows.Add(r.categoria, r.probabilidad * 100);
                    probFinales.Add(new resN(r.probabilidad * 100, r.categoria));
                }


                List<resN> SortedList = probFinales.OrderBy(o => o.prob).ToList();
                SortedList.Reverse();

                string a = "";

                if (SortedList.ElementAt(0).prob > 80)
                {
                    a = aprendizaje.aprender(text_area.Text, 1, SortedList.ElementAt(0).cat, true, true);
                }

                categoria.Text = "La categoria es: " + SortedList.ElementAt(0).cat + " con un " + SortedList.ElementAt(0).prob + "%";
                response.Text = a;
                LiteralCateTexto.Text = drc.BindChart(table, "chartCateTexto", "Porcentaje de Categorias", "Categorias");                                
            }

        }

        private class resN
        {
            public double prob;
            public string cat;
            public resN(double prob, string cat)
            {
                this.cat = cat;
                this.prob = prob;
            }
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
            IA.aprender.Aprender.ListaDeTweets.Clear();
            IA.aprender.Aprender.vienenTweets = true;
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
                IA.aprender.Aprender.ListaDeTweets.Clear();
                IA.aprender.Aprender.vienenPosts = true;
                text_area.Text = GetTweets.getTweet(text_area.Text.Split(' ')[0], Int32.Parse(text_area.Text.Split(' ')[1]));
            }
        }        
    }
}