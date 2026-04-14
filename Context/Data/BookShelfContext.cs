using Microsoft.EntityFrameworkCore;
using BookShelfAPI.Models;

namespace BookShelfAPI.Data;

public class BookShelfContext : DbContext
{
    public BookShelfContext(DbContextOptions<BookShelfContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Livro> Livros { get; set; }
    public DbSet<MetaLeitura> MetasLeitura { get; set; }
    public DbSet<MetaLeituraLivro> MetaLeituraLivros { get; set; }
    public DbSet<DesafioAZ> DesafiosAZ { get; set; }
    public DbSet<LetraDesafio> LetrasDesafio { get; set; }
    public DbSet<CalendarioMensal> CalendariosMensais { get; set; }
    public DbSet<ProximoLeitura> ProximasLeituras { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração Usuario -> Livros
        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Livro)
            .WithOne(l => l.Usuario)
            .HasForeignKey(l => l.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração Usuario -> MetaLeitura (1:1)
        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.MetaLeitura)
            .WithOne(m => m.Usuario)
            .HasForeignKey<MetaLeitura>(m => m.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração Usuario -> DesafioAZ (1:1)
        modelBuilder.Entity<Usuario>()
            .HasOne(u => u.DesafioAZ)
            .WithOne(d => d.Usuario)
            .HasForeignKey<DesafioAZ>(d => d.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração Usuario -> CalendarioMensal
        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.CalendariosMensais)
            .WithOne(c => c.Usuario)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração DesafioAZ -> Letras
        modelBuilder.Entity<DesafioAZ>()
            .HasMany(d => d.Letras)
            .WithOne(l => l.DesafioAZ)
            .HasForeignKey(l => l.DesafioAZId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração MetaLeitura -> Livros (Many-to-Many)
        modelBuilder.Entity<MetaLeitura>()
            .HasMany(m => m.LivrosNaMeta)
            .WithOne(ml => ml.MetaLeitura)
            .HasForeignKey(ml => ml.MetaLeituraId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MetaLeituraLivro>()
            .HasKey(ml => new { ml.MetaLeituraId, ml.LivroId });

        // Índices únicos
        modelBuilder.Entity<CalendarioMensal>()
            .HasIndex(c => new { c.UsuarioId, c.Ano, c.Mes })
            .IsUnique();

        modelBuilder.Entity<MetaLeitura>()
            .HasIndex(m => new { m.UsuarioId, m.Ano })
            .IsUnique();

        modelBuilder.Entity<DesafioAZ>()
            .HasIndex(d => new { d.UsuarioId, d.Ano })
            .IsUnique();
    }
}
