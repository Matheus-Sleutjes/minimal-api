using Microsoft.EntityFrameworkCore;
using MinimalAPI.Domain;

namespace MinimalAPI.Data
{
    public class LivrosContext : DbContext
    {
        public LivrosContext(DbContextOptions<LivrosContext> options) : base(options) { }

        public DbSet<Livro> Livros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5000;Database=livros-db;Username=postgres;Password=admin");
    }
}
