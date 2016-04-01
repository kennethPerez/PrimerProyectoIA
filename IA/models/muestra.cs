using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IA.models
{
    public class muestra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int muestrasID { get; set; }
        public string categoria;
        public int IDdioma;
        public List<Palabras> palabra;
        public List<List<Palabras>> listaDelistasDepalabras = new List<List<Palabras>>();//para mejorar rendimiento perder memoria
        public muestra(string categoria, List<Palabras> palabra)
        {
            this.categoria = categoria;
            this.palabra = palabra;
        }
    }
}