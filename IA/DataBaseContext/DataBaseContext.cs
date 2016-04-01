using IA.models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace IA.DataBaseContext
{
    public class IAContext : DbContext
    {

        public IAContext() : base("IAContext")
        {
        }

        public DbSet<muestra> muestra { get; set; }
        public DbSet<Idiomas> Idiomas { get; set; }
        public DbSet<Palabras> palabras { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}