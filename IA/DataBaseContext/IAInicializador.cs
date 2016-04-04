using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using IA.models;

namespace IA.DataBaseContext
{
    public class IAInicializador : DropCreateDatabaseIfModelChanges<IAContext>
    {
        protected override void Seed(IAContext context)
        {
            var idiomas = new List<Idiomas>
            {
                new Idiomas{Idioma="Español"},
                new Idiomas{Idioma="Ingles"},
                new Idiomas{Idioma="Aleman"},
                new Idiomas{Idioma="Turco"}
            };
            idiomas.ForEach(s => context.Idiomas.Add(s));
            context.SaveChanges();



            var PalabrasEs = new List<Palabras>
            {
                new Palabras{palabra="tecnologia",IDdioma=1},
                new Palabras{palabra="computacion",IDdioma=1},
                new Palabras{palabra="biblia",IDdioma=1},
                new Palabras{palabra="religion",IDdioma=1},
                new Palabras{palabra="asesino",IDdioma=1},
                new Palabras{palabra="arma",IDdioma=1},
                new Palabras{palabra="celular",IDdioma=1},
                new Palabras{palabra="muerte",IDdioma=1},
            };
            PalabrasEs.ForEach(s => context.palabras.Add(s));
            context.SaveChanges();
            /*var PalabrasGe = new List<Palabras>
            {
                new Palabras{palabra="aleman",IDdioma=3},
                new Palabras{palabra="aleman",IDdioma=3},
                new Palabras{palabra="aleman",IDdioma=3},
                new Palabras{palabra="aleman",IDdioma=3},
                new Palabras{palabra="aleman",IDdioma=3},
                new Palabras{palabra="aleman",IDdioma=3},
                new Palabras{palabra="aleman",IDdioma=3},
                new Palabras{palabra="aleman",IDdioma=3},
            };
            PalabrasGe.ForEach(s => context.palabras.Add(s));
            context.SaveChanges();*/
            /*Nueva Muestra*/
            var MuestraX = new List<muestra>
            {
                new muestra{categoria="Tecnologia",IDdioma=1},
                new muestra{categoria="Tecnologia",IDdioma=1},
                new muestra{categoria="Religion",IDdioma=1},
                new muestra{categoria="Religion",IDdioma=1},
                new muestra{categoria="Sucesos",IDdioma=1},
                new muestra{categoria="Sucesos",IDdioma=1}
            };
            MuestraX.ForEach(s => context.muestra.Add(s));
            context.SaveChanges();

            var RelacionMuestraPalabras = new List<relacion>
            {
                /*Para cada muestra con todas las palabras del mismo idioma*/
                /*Muestra 1*/
                new relacion{muestraID=1,palabraID=1,frecuencia=2},
                new relacion{muestraID=1,palabraID=2,frecuencia=2},
                new relacion{muestraID=1,palabraID=3,frecuencia=0},
                new relacion{muestraID=1,palabraID=4,frecuencia=1},
                new relacion{muestraID=1,palabraID=5,frecuencia=1},
                new relacion{muestraID=1,palabraID=6,frecuencia=0},
                new relacion{muestraID=1,palabraID=7,frecuencia=2},
                new relacion{muestraID=1,palabraID=8,frecuencia=1},
                /*Para cada muestra con todas las palabras del mismo idioma*/
                /*Muestra 2*/
                new relacion{muestraID=2,palabraID=1,frecuencia=3},
                new relacion{muestraID=2,palabraID=2,frecuencia=3},
                new relacion{muestraID=2,palabraID=3,frecuencia=1},
                new relacion{muestraID=2,palabraID=4,frecuencia=0},
                new relacion{muestraID=2,palabraID=5,frecuencia=0},
                new relacion{muestraID=2,palabraID=6,frecuencia=1},
                new relacion{muestraID=2,palabraID=7,frecuencia=3},
                new relacion{muestraID=2,palabraID=8,frecuencia=0},
                /*Para cada muestra con todas las palabras del mismo idioma*/
                /*Muestra 3*/
                new relacion{muestraID=3,palabraID=1,frecuencia=1},
                new relacion{muestraID=3,palabraID=2,frecuencia=1},
                new relacion{muestraID=3,palabraID=3,frecuencia=2},
                new relacion{muestraID=3,palabraID=4,frecuencia=2},
                new relacion{muestraID=3,palabraID=5,frecuencia=1},
                new relacion{muestraID=3,palabraID=6,frecuencia=1},
                new relacion{muestraID=3,palabraID=7,frecuencia=1},
                new relacion{muestraID=3,palabraID=8,frecuencia=2},
                /*Para cada muestra con todas las palabras del mismo idioma*/
                /*Muestra 4*/
                new relacion{muestraID=4,palabraID=1,frecuencia=0},
                new relacion{muestraID=4,palabraID=2,frecuencia=0},
                new relacion{muestraID=4,palabraID=3,frecuencia=3},
                new relacion{muestraID=4,palabraID=4,frecuencia=3},
                new relacion{muestraID=4,palabraID=5,frecuencia=0},
                new relacion{muestraID=4,palabraID=6,frecuencia=0},
                new relacion{muestraID=4,palabraID=7,frecuencia=0},
                new relacion{muestraID=4,palabraID=8,frecuencia=1},
                /*Para cada muestra con todas las palabras del mismo idioma*/
                /*Muestra 5*/
                new relacion{muestraID=5,palabraID=1,frecuencia=1},
                new relacion{muestraID=5,palabraID=2,frecuencia=1},
                new relacion{muestraID=5,palabraID=3,frecuencia=1},
                new relacion{muestraID=5,palabraID=4,frecuencia=1},
                new relacion{muestraID=5,palabraID=5,frecuencia=2},
                new relacion{muestraID=5,palabraID=6,frecuencia=2},
                new relacion{muestraID=5,palabraID=7,frecuencia=1},
                new relacion{muestraID=5,palabraID=8,frecuencia=2},
                /*Para cada muestra con todas las palabras del mismo idioma*/
                /*Muestra 6*/
                new relacion{muestraID=6,palabraID=1,frecuencia=0},
                new relacion{muestraID=6,palabraID=2,frecuencia=0},
                new relacion{muestraID=6,palabraID=3,frecuencia=0},
                new relacion{muestraID=6,palabraID=4,frecuencia=0},
                new relacion{muestraID=6,palabraID=5,frecuencia=3},
                new relacion{muestraID=6,palabraID=6,frecuencia=1},
                new relacion{muestraID=6,palabraID=7,frecuencia=0},
                new relacion{muestraID=6,palabraID=8,frecuencia=3},
            };
            RelacionMuestraPalabras.ForEach(s => context.relacion.Add(s));
            context.SaveChanges();



        }
    }
}