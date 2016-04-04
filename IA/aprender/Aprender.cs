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

        IA.stopWords.stopWords stopWords = new IA.stopWords.stopWords();
        private IAContext db = new IAContext();
        public string aprenderMuestra(bayesCategoria muestra,int IdIdioma)
        {
            try
            {
                db.muestra.Add(new muestra { categoria = muestra.categoria, IDdioma = IdIdioma });
                db.SaveChanges();
                int muestraIDAInsertar = db.muestra.Max(t => t.muestrasID);
                var allPalabras = db.palabras.ToArray();
                foreach (bayesPalabra palabra in muestra.palabra)
                {
                    foreach (Palabras x in allPalabras)
                    {
                        if (x.palabra.Equals(palabra.palabra))
                        {
                            db.relacion.Add(new relacion { palabraID = x.palabraID, muestraID = muestraIDAInsertar, frecuencia = Convert.ToInt32(palabra.frecuencia) });
                        }
                    }
                }
                db.SaveChanges();
                return "Exito en la insercion de una muestra";
            }
            catch (Exception e)
            {
                return "Error en la insercion de una muestra"+" --> "+e.Message;
            }


            
        }
        public string aprenderPalabras(List<incidencia> palabras,int idiomaID)
        {
            try {
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
        public string aprender(string Text,int IdIdiomaN,string cat)
        {

            /*Faltan validaciones*/
            string texto = Text.ToLower();
            int IdIdioma = IdIdiomaN;
            string categoria = cat;

            /*Filtrado de palabras*/
            List<incidencia> incidencias = new List<incidencia>();
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string[] words = texto.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
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


            string Res = "";
            Res =   "\n" + aprenderPalabras(PalabrasAAgregar, IdIdioma);

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
                palabrasMuestra.Add(new bayesPalabra(palabra.palabra, System.Text.RegularExpressions.Regex.Matches(texto, palabra.palabra).Count));
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

    }
}