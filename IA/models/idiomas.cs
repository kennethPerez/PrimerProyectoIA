using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IA.models
{
    public class Idiomas
    {
        [Key]
        public int idiomaID { get; set; }
        public string Idioma { get; set; }
    }
}