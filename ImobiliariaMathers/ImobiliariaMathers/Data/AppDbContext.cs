using ImobiliariaMathers.Models;
using Microsoft.EntityFrameworkCore;

namespace ImobiliariaMathers.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<CodigoRecuperacao> CodigosRecuperacao => Set<CodigoRecuperacao>();
        public DbSet<Imovel> Imoveis => Set<Imovel>();
        public DbSet<Imagem> Imagens => Set<Imagem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();

                entity.Property(u => u.Tipo)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.Property(u => u.CriadoEm)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CodigoRecuperacao>(entity =>
            {
                entity.HasIndex(c => new { c.UsuarioId, c.CodigoHash }).IsUnique();

                entity.Property(c => c.CriadoEm)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();

                entity.HasOne(c => c.Usuario)
                    .WithMany()
                    .HasForeignKey(c => c.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Imovel>(entity =>
            {
                entity.Property(i => i.Tipo)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.Property(i => i.Negocio)
                    .HasConversion<string>()
                    .HasMaxLength(10);

                entity.Property(i => i.Estado)
                    .HasColumnType("char(2)")
                    .HasMaxLength(2);

                entity.Property(i => i.CriadoEm)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();

                entity.Property(i => i.AtualizadoEm)
                    .HasColumnType("timestamp")
                    .IsRequired(false);
            });

            modelBuilder.Entity<Imagem>(entity =>
            {
                entity.Property(i => i.CriadoEm)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();

                entity.HasOne(i => i.Imovel)
                    .WithMany(i => i.Imagens)
                    .HasForeignKey(i => i.ImovelId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}