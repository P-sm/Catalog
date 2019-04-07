using Microsoft.EntityFrameworkCore;

namespace Сatalog.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(){ }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        
        public DbSet<Dept> Depts { get; set; }
        public DbSet<Worker> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Username=postgres;Password=RrTtYy1739;Database=company_directory;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){ }

    }
}
