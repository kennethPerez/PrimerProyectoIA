using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IA.models
{
    public class Palabras
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int palabraID { get; set; }
        public string palabra;
        public double frecuencia;
        public int IDdioma;
        public double calculateMedia;
        public double calculateVarianza;
        public Palabras(string palabra, double frecuencia)
        {
            this.frecuencia = frecuencia;
            this.palabra = palabra;
        }
        public Palabras(string palabra, double calculateMedia, double calculateVarianza)
        {
            this.palabra = palabra;
            this.calculateMedia = calculateMedia;
            this.calculateVarianza = calculateVarianza;
        }
    }
}