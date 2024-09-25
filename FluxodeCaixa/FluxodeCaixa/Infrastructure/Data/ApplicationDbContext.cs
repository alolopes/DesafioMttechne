using FluxoDeCaixa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FluxoDeCaixa.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<Consolidacao> Consolidacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Consolidacao>();
            modelBuilder.Entity<Lancamento>().ToTable("Lancamentos");
        }
    }
}
