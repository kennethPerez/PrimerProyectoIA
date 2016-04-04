using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IA.models
{
    public class relacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int relacionID { get; set; }
        public int muestraID { get; set; }
        public int palabraID { get; set; }
        public int frecuencia { get; set; }
    }
}