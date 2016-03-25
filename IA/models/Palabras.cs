using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IA.models
{
    public class Palabras
    {
        [Key]
        public int palabraID { get; set; }
        public string palabra { get; set; }
        public int idiomaID { get; set; }
        public int categoriaID { get; set; }

    }
}