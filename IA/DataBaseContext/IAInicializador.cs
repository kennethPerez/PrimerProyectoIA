using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using IA.models;

namespace IA.DataBaseContext
{
    public class IAInicializador : DropCreateDatabaseAlways<IAContext>
    {
        protected override void Seed(IAContext context)
        {
            
        }
    }
}