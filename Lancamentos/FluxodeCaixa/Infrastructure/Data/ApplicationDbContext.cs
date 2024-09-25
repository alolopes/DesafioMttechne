using Lancamentos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lancamentos.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Lancamento> Lancamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lancamento>().ToTable("Lancamentos");
        }
    }
}
