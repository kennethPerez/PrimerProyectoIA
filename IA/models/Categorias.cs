using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IA.models
{
    public class Categorias
    {
        [Key]
        public int categoriaID { get; set; }
        public string Categoria { get; set; }
    }
}