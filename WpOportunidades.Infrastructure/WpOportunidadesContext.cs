using Microsoft.EntityFrameworkCore;
using System;
using WpOportunidades.Entities;

namespace WpOportunidades.Infrastructure
{
    public class WpOportunidadesContext : DbContext
    {
        public DbSet<Oportunidade> Oportunidades { get; set; }
        public DbSet<UserXOportunidade> UserXOportunidades { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<OportunidadeStatus> OportunidadeStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=TSERVICES\SQLEXPRESS;Database=WebPixOportunidades;Trusted_Connection=True;Integrated Security = True;");
            //optionsBuilder.UseSqlServer(@"Data Source=18.229.17.132;Initial Catalog=WebPixOportunidades;Persist Security Info=True;User ID=sa;Password=StaffPro@123;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Oportunidade>(op => 
            {
                op.Property(o => o.Nome).HasColumnType("varchar(50)");
                op.Property(o => o.Descricao).HasColumnType("varchar(200)");
                op.Property(o => o.DescProfissional).HasColumnType("varchar(200)");
                op.Property(o => o.DescServico).HasColumnType("varchar(200)");
            });

            modelBuilder.Entity<Endereco>(end => 
            {
                end.Property(e => e.Nome).HasColumnType("varchar(50)");
                end.Property(e => e.Descricao).HasColumnType("varchar(200)");
                end.Property(e => e.CEP).HasColumnType("varchar(9)");
                end.Property(e => e.Estado).HasColumnType("varchar(20)");
                end.Property(e => e.Cidade).HasColumnType("varchar(20)");
                end.Property(e => e.Bairro).HasColumnType("varchar(20)");
                end.Property(e => e.Local).HasColumnType("varchar(50)");
                end.Property(e => e.Complemento).HasColumnType("varchar(100)");
                end.Property(e => e.LocalOportunidade).HasColumnType("varchar(100)");
            });
        }
    }
}
