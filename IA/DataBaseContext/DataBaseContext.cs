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

        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Idiomas> Idiomas { get; set; }
        public DbSet<Palabras> Palabras { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}