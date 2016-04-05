using IA.bayes_algoritmo;
using IA.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IA.models;

namespace IA.aprender
{
    public class Aprender
    {
        public static bool vienenTweets = false;
        IA.stopWords.stopWords stopWords = new IA.stopWords.stopWords();
        private IAContext db = new IAContext();
        public string aprenderMuestra(bayesCategoria muestra, int IdIdioma)
        {
            try
            {
                db.muestra.Add(new muestra { categoria = muestra.categoria, IDdioma = IdIdioma });
                db.SaveChanges();
                int muestraIDAInsertar = db.muestra.Max(t => t.muestrasID);
                var allPalabras = from a in db.palabras join b in db.Idiomas on a.IDdioma equals b.idiomaID where b.idiomaID == IdIdioma select new { palabra = a.palabra, palabraId=a.palabraID };

                //var allPalabras = db.palabras.ToArray();
                int i = 0;
                foreach (var palabra in allPalabras) { 
                    if (palabra.palabra.Equals(muestra.palabra.ElementAt(i).palabra))
                    {
                        db.relacion.Add(new relacion { palabraID = palabra.palabraId, muestraID = muestraIDAInsertar, frecuencia = Convert.ToInt32(muestra.palabra.ElementAt(i).frecuencia) });
                    }
                    i++;
                }
                db.SaveChanges();
                return "Exito en la insercion de una muestra";
            }
            catch (Exception e)
            {
                return "Error en la insercion de una muestra" + " --> " + e.Message;
            }



        }
        public string aprenderPalabras(List<incidencia> palabras, int idiomaID)
        {
            try
            {
                foreach (incidencia palabra in palabras)
                {
                    db.palabras.Add(new IA.models.Palabras { IDdioma = idiomaID, palabra = palabra.palabra });
                    db.SaveChanges();
                    int PalabraIDAInsertar = db.palabras.Max(t => t.palabraID);

                    foreach (IA.models.muestra muestra in db.muestra)
                    {
                        db.relacion.Add(new relacion { palabraID = PalabraIDAInsertar, muestraID = muestra.muestrasID, frecuencia = 0 });
                    }
                    db.SaveChanges();
                }
                return "Exito  la insercion de palabras";
            }
            catch (Exception e)
            {
                return "Error en la insercion de una palabra" + " --> " + e.Message;
            }



        }
        public string aprender(string Text, int IdIdiomaN, string cat, Boolean palabras)
        {

            /*Faltan validaciones*/

            string Res = "";
            string texto = Text.ToLower();
            int IdIdioma = IdIdiomaN;
            string categoria = cat;
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] words = texto.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
            if (palabras)
            {
                /*Filtrado de palabras*/
                List<incidencia> incidencias = new List<incidencia>();
                
                string[] distinctWords = words.Distinct().ToArray();
                foreach (string word in distinctWords)
                {
                    incidencias.Add(new incidencia(word, cantidadApariciones(word, words)));
                }

                List<incidencia> SortedList = incidencias.OrderBy(o => o.repeticiones).ToList();
                SortedList.Reverse();
                /*Filtrarlas por las stop words*/
                List<string> StopWord = new List<string>();

                if (IdIdioma == 1)
                {
                    StopWord = stopWords.stopWordSpanish;
                }
                if (IdIdioma == 2)
                {
                    StopWord = stopWords.stopWordEnglish;
                }
                if (IdIdioma == 3)
                {
                    StopWord = stopWords.stopWordGerman;
                }
                if (IdIdioma == 4)
                {
                    StopWord = stopWords.stopWordTurkish;
                }

                //no esta funckando
                List<incidencia> PalabrasSinStopWords = new List<incidencia>();
                foreach (string comp in StopWord)
                {
                    SortedList.RemoveAll(u => u.palabra.Equals(comp));
                }

                /*filtrar que no meta palabras repetidas*/

                foreach (IA.models.Palabras comp in db.palabras)
                {
                    SortedList.RemoveAll(u => u.palabra.Equals(comp.palabra));
                }

                /*seleccionar cuantas aprender*/
                List<incidencia> PalabrasAAgregar = new List<incidencia>();
                int cantidadDeApredisagePalabras = 4;
                for (int i = 0; i < cantidadDeApredisagePalabras; i++)
                {
                    PalabrasAAgregar.Add(SortedList.ElementAt(i));
                }


                Res = "\n" + aprenderPalabras(PalabrasAAgregar, IdIdioma);
            }
            /*Generacion de la muestra*/
            List<bayesPalabra> tableResult = new List<bayesPalabra>();

            var query = from a in db.palabras join b in db.Idiomas on a.IDdioma equals b.idiomaID where b.idiomaID == IdIdioma select new { palabra = a.palabra };


            foreach (var palabra in query)
            {
                tableResult.Add(new bayesPalabra(palabra.palabra, 0));
            }



            List<bayesPalabra> palabrasMuestra = new List<bayesPalabra>();

            foreach (bayesPalabra palabra in tableResult)
            {
                palabrasMuestra.Add(new bayesPalabra(palabra.palabra, cantidadApariciones( palabra.palabra, words)));
            }

            bayesCategoria tablaMuestra = new bayesCategoria(categoria, palabrasMuestra);
            Res = Res + "\n" + aprenderMuestra(tablaMuestra, IdIdioma);
            return Res;
        }

        private int cantidadApariciones(string palabra, string[] lista)
        {
            int i = 0;
            foreach (string palabraEnLista in lista)
            {
                if (palabraEnLista.Equals(palabra))
                {
                    i++;
                }
            }
            return i;
        }

        public void generarUnosParaCategoria(string cat, int IdIdioma)
        {

            var query = from a in db.palabras join b in db.Idiomas on a.IDdioma equals b.idiomaID where b.idiomaID == IdIdioma select new { palabra = a.palabra, palabraId=a.palabraID };

            db.muestra.Add(new muestra { categoria = cat, IDdioma = IdIdioma });
           
            db.SaveChanges();
            int muestraIDAInsertar = db.muestra.Max(t => t.muestrasID);

            foreach (var palabra in query)
            {
                db.relacion.Add(new relacion { palabraID = palabra.palabraId, muestraID = muestraIDAInsertar, frecuencia = 1 });
            }
            db.SaveChanges();
        }

        public void aprendaUrls()
        {
            Lecturas.UrlUploader urlConvert = new Lecturas.UrlUploader();


            generarUnosParaCategoria("Tecnologia", 1);
            /**
            aprender(urlConvert.ParserUrl("http://www.nacion.com/vivir/ciencia/Crece-interes-tecnologia-interpreta-emociones_0_1552844727.html"), 1, "Tecnologia", false);
            aprender(urlConvert.ParserUrl("http://www.nacion.com/tecnologia/celulares/nuevos-telefonos-Samsung-llegan-pais_0_1552044793.html"), 1, "Tecnologia", false);
            aprender(urlConvert.ParserUrl("http://www.nacion.com/tecnologia/informatica/Concurso-Robotifest-UCR-abre-convocatoria_0_1552844712.html"), 1, "Tecnologia", false);
            aprender(urlConvert.ParserUrl("http://www.nacion.com/tecnologia/avances/Laboratorio-Veritas-trabaja-tiburon_0_1552844725.html"), 1, "Tecnologia", false);
            aprender(urlConvert.ParserUrl("http://www.nacion.com/vivir/ciencia/NASA-invita-ticos-disenar-aplicaciones_0_1552844716.html"), 1, "Tecnologia", false);
            aprender(urlConvert.ParserUrl("http://www.nacion.com/tecnologia/avances/Microsoft-obligado-retirar-chatbot_0_1552644744.html"), 1, "Tecnologia", false);
            aprender(urlConvert.ParserUrl("http://www.nacion.com/tecnologia/avances/America-Latina-zaga-teledeteccion-remota_0_1552444825.html"), 1, "Tecnologia", false);
            aprender(urlConvert.ParserUrl("http://www.nacion.com/tecnologia/celulares/Apple-alcanza-decadas_0_1552044797.html"), 1, "Tecnologia", false);
            aprender(urlConvert.ParserUrl("http://www.nacion.com/tecnologia/redes-sociales/youtube-youtube_kids-ninos-eduacion-diversion-espanol_0_1551844899.html"), 1, "Tecnologia", false);
            aprender(urlConvert.ParserUrl("http://www.nacion.com/tecnologia/gadgets/Prendas-inteligentes-botin-datos-ciberdelincuentes_0_1551644847.html"), 1, "Tecnologia", false);
           
            aprender(urlConvert.ParserUrl("nafda"), 1, "Sucesos", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Sucesos", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Sucesos", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Sucesos", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Sucesos", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Sucesos", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Sucesos", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Sucesos", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Sucesos", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Sucesos", false);

            aprender(urlConvert.ParserUrl("nafda"), 1, "Economia", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Economia", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Economia", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Economia", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Economia", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Economia", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Economia", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Economia", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Economia", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Economia", false);

            aprender(urlConvert.ParserUrl("nafda"), 1, "Deportes", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Deportes", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Deportes", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Deportes", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Deportes", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Deportes", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Deportes", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Deportes", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Deportes", false);
            aprender(urlConvert.ParserUrl("nafda"), 1, "Deportes", false);**/

        }

    }






}