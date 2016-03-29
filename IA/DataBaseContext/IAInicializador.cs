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
                new Categorias{Categoria="Deporte"},
                new Categorias{Categoria="Politica"}
            };
            categorias.ForEach(s => context.Categorias.Add(s));
            context.SaveChanges();

            var idiomas = new List<Idiomas>
            {
                new Idiomas{Idioma="Ingles"},
                new Idiomas{Idioma="Espannol"}
            };
            idiomas.ForEach(s => context.Idiomas.Add(s));
            context.SaveChanges();

            var palabras = new List<Palabras>
            {
                new Palabras{palabra="Balon",idiomaID=1,categoriaID=0},
                new Palabras{palabra="Presidente",idiomaID=1,categoriaID=1},
            };
            palabras.ForEach(s => context.Palabras.Add(s));
            context.SaveChanges();
        }
    }
}