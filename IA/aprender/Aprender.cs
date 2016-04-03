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
        private IAContext db = new IAContext();
        public string aprenderMuestra(bayesCategoria muestra,int IdIdioma)
        {
            try
            {
                db.muestra.Add(new muestra { categoria = muestra.categoria, IDdioma = IdIdioma });
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
                return "Exito en la insercion de una muestra";
            }
            catch (Exception e)
            {
                return "Error en la insercion de una muestra"+" --> "+e.Message;
            }


            
        }
        public string aprenderPalabras(List<bayesPalabra> palabras)
        {

            return "Exito";
        }
    }
}