using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Сatalog
{
    public partial class company_directoryContext : DbContext
    {
        public company_directoryContext()
        {
        }

        public company_directoryContext(DbContextOptions<company_directoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Depts> Depts { get; set; }
        public virtual DbSet<Workers> Workers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=5432;Username=postgres;Password=RrTtYy1739;Database=company_directory;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {}
    }
}
