using Microsoft.EntityFrameworkCore;
using minimal_api_todo.Models;

namespace minimal_api_todo.Data { 
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
        public DbSet<AtividadeModel> Atividades { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AtividadeModel>()
                .ToTable("tb_cad_atividade");
            modelBuilder.Entity<AtividadeModel>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<AtividadeModel>()
                        .Property(p => p.Titulo)
                        .IsRequired()
                        .HasColumnType("varchar(100)");
            modelBuilder.Entity<AtividadeModel>()
                        .Property(p => p.Descricao)
                        .IsRequired()
                        .HasColumnType("varchar(250)");
            modelBuilder.Entity<AtividadeModel>()
                        .Property(p => p.Status)
                        .IsRequired()
                        .HasColumnType("integer");

            base.OnModelCreating(modelBuilder);
        }
    }
}
