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

        public void generarUnosParaCategoria(int num,string cat, int IdIdioma)
        {

            var query = from a in db.palabras join b in db.Idiomas on a.IDdioma equals b.idiomaID where b.idiomaID == IdIdioma select new { palabra = a.palabra, palabraId=a.palabraID };

            db.muestra.Add(new muestra { categoria = cat, IDdioma = IdIdioma });
           
            db.SaveChanges();
            int muestraIDAInsertar = db.muestra.Max(t => t.muestrasID);

            foreach (var palabra in query)
            {
                db.relacion.Add(new relacion { palabraID = palabra.palabraId, muestraID = muestraIDAInsertar, frecuencia = num });
            }
            db.SaveChanges();
        }

        public void aprendaUrls()
        {
            Lecturas.UrlUploader urlConvert = new Lecturas.UrlUploader();


            /*generarUnosParaCategoria(0,"Tecnologia", 1);
            
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
            */
            /*
             generarUnosParaCategoria(0,"Sucesos", 1);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/sucesos/seguridad/Padrastro-detenido-violar-menor-discapacidad_0_1552644844.html"), 1, "Sucesos", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/sucesos/poder-judicial/corte-gas_zeta-sala_primera_0_1552644849.html"), 1, "Sucesos", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/sucesos/crimenes-asaltos/Impacto_de_bala-Guacimo-Fallecido_0_1552444863.html"), 1, "Sucesos", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/sucesos/narcotrafico/Colombia-decomisa-tonelada-coca-venia_0_1552844737.html"), 1, "Sucesos", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/sucesos/accidentes/Nina-cae_pozo_de_agua-San_Carlos_0_1552644799.html"), 1, "Sucesos", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/sucesos/crimenes-asaltos/Playas_del_Coco-crimen_de_Alejo_Leiva_0_1552644838.html"), 1, "Sucesos", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/sucesos/seguridad/Unidades-Caninas-salva-combate-crimen_0_1544245622.html"), 1, "Sucesos", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/sucesos/Red-intento-extorsionar-cayeron-seduccion_0_1552044877.html"), 1, "Sucesos", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/sucesos/accidentes/Adolescente-muere-golpeado-rina_0_1552644823.html"), 1, "Sucesos", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/sucesos/seguridad/allanamiento-m_express-alajuela_0_1551844858.html"), 1, "Sucesos", false);*/

            /*
             generarUnosParaCategoria(0,"Economia", 1);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/economia/politica-economica/Costa-Rica-rinde-informe-OCDE_0_1552844731.html"), 1, "Economia", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/economia/indicadores/Especialistas-preven-estabilidad-dolar_0_1552844728.html"), 1, "Economia", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/economia/empresarial/papeles-panama-nacion-costa-rica_0_1552844745.html"), 1, "Economia", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/economia/finanzas/Desempleo-EE-UU-sube-marzo_0_1552644784.html"), 1, "Economia", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/economia/empresarial/Alvaro-Cedeno-Desempleo-estructural_0_1552644732.html"), 1, "Economia", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/economia/finanzas/Francia-incluir-Panama-paraisos-fiscales_0_1552844763.html"), 1, "Economia", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/economia/empresarial/Contratacion_administrativa-compras_publicas-aplicacion-Estado_0_1552044874.html"), 1, "Economia", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/economia/finanzas/Ricardo-Gonzalez-centros-recreo_0_1552644731.html"), 1, "Economia", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/economia/empresarial/Honduras-firma-contrato-construir-aeropuerto_0_1552044804.html"), 1, "Economia", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/economia/empresarial/internacionales-capacitaran-empresarios-Expo-Pyme_0_1552644825.html"), 1, "Economia", false);


             generarUnosParaCategoria(0,"Deportes", 1);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/deportes/futbol-costa-rica/Alajuelense-Johnny-Acosta-meniscos-Seleccion_0_1552844758.html"), 1, "Deportes", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/deportes/futbol-costa-rica/Hernan-Medford-echa-dominio-Alajuelense_0_1552844746.html"), 1, "Deportes", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/deportes/legionarios/Keylor_Navas-Legionarios-Real_Madrid-Champions_League-Wolfsburg_0_1552644813.html"), 1, "Deportes", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/deportes/ciclismo/Andrey-Amador-echara-Pizza-Hut_0_1552844723.html"), 1, "Deportes", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/deportes/legionarios/Bryan-Ruiz-Sporting-Lisboa-ultimo_0_1552844715.html"), 1, "Deportes", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/deportes/futbol-costa-rica/Juan-Carlos-Rojas-Jorge-Vergara_0_1552644811.html"), 1, "Deportes", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/deportes/futbol-costa-rica/Carlos_Chamberlain-Alajuelense_0_1552644798.html"), 1, "Deportes", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/deportes/legionarios/Keylor-Navas-luce-jugada-escorpion_0_1552644812.html"), 1, "Deportes", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/deportes/futbol-costa-rica/Reportaje-fotografico-Cueva-basurero-estadio_0_1552644804.html"), 1, "Deportes", false);
             aprender(urlConvert.ParserUrl("http://www.nacion.com/deportes/futbol-costa-rica/Alajuelense-presion-Carmelita-despegarse-punteros_0_1552644847.html"), 1, "Deportes", false);
             */
            generarUnosParaCategoria(0, "Economia", 1);
            generarUnosParaCategoria(0, "Deportes", 1);
            generarUnosParaCategoria(0, "Sucesos", 1);
            generarUnosParaCategoria(0, "Tecnologia", 1);
        }

    }






}