using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using IA.models;

namespace IA.DataBaseContext
{
    public class IAInicializador : System.Data.Entity.DropCreateDatabaseIfModelChanges<IAContext>
    {
        protected override void Seed(IAContext context)
        {
            var categorias = new List<Categorias>
            {
                new Categorias{Categoria="Deporte",categoriaID=0},
                new Categorias{Categoria="Politica",categoriaID=1}
            };
            categorias.ForEach(s => context.Categorias.Add(s));
            context.SaveChanges();

            var idiomas = new List<Idiomas>
            {
                new Idiomas{Idioma="Ingles",idiomaID=0},
                new Idiomas{Idioma="Espannol",idiomaID=1}
            };
            idiomas.ForEach(s => context.Idiomas.Add(s));
            context.SaveChanges();

            var palabras = new List<Palabras>
            {
                new Palabras{palabra="Balon",palabraID=0,idiomaID=1,categoriaID=0},
                new Palabras{palabra="Presidente",palabraID=1,idiomaID=1,categoriaID=1},
            };
            palabras.ForEach(s => context.Palabras.Add(s));
            context.SaveChanges();
        }
    }
}