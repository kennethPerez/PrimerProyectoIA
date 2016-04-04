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
        public string categoria { get; set; }
        public int IDdioma { get; set; }
        
    }
}