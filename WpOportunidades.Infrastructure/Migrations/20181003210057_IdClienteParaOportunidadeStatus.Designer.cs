﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WpOportunidades.Infrastructure;

namespace WpOportunidades.Infrastructure.Migrations
{
    [DbContext(typeof(WpOportunidadesContext))]
    [Migration("20181003210057_IdClienteParaOportunidadeStatus")]
    partial class IdClienteParaOportunidadeStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WpOportunidades.Entities.Endereco", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo");

                    b.Property<string>("Bairro")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CEP")
                        .HasColumnType("varchar(9)");

                    b.Property<string>("Cidade")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Complemento")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("DataCriacao");

                    b.Property<DateTime>("DataEdicao");

                    b.Property<string>("Descricao")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Estado")
                        .HasColumnType("varchar(20)");

                    b.Property<int>("IdCliente");

                    b.Property<int>("IdUsuario");

                    b.Property<string>("Local")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Nome")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("NumeroLocal");

                    b.Property<int>("OportunidadeId");

                    b.Property<int>("Status");

                    b.Property<int>("UsuarioCriacao");

                    b.Property<int>("UsuarioEdicao");

                    b.HasKey("ID");

                    b.HasIndex("OportunidadeId")
                        .IsUnique();

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("WpOportunidades.Entities.Oportunidade", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo");

                    b.Property<DateTime>("DataCriacao");

                    b.Property<DateTime>("DataEdicao");

                    b.Property<DateTime>("DataOportunidade");

                    b.Property<string>("DescProfissional")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("DescServico")
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Descricao")
                        .HasColumnType("varchar(200)");

                    b.Property<TimeSpan>("HoraFim");

                    b.Property<TimeSpan>("HoraInicio");

                    b.Property<int>("IdCliente");

                    b.Property<int>("IdEmpresa");

                    b.Property<string>("Nome")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("OportunidadeStatusID");

                    b.Property<int>("Quantidade");

                    b.Property<int>("Status");

                    b.Property<int>("UsuarioCriacao");

                    b.Property<int>("UsuarioEdicao");

                    b.Property<decimal>("Valor");

                    b.HasKey("ID");

                    b.HasIndex("OportunidadeStatusID");

                    b.ToTable("Oportunidades");
                });

            modelBuilder.Entity("WpOportunidades.Entities.OportunidadeStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdCliente");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("OportunidadeStatuses");
                });

            modelBuilder.Entity("WpOportunidades.Entities.Status", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("WpOportunidades.Entities.UserXOportunidade", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OportunidadeId");

                    b.Property<int>("StatusID");

                    b.Property<int>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("OportunidadeId");

                    b.HasIndex("StatusID");

                    b.ToTable("UserXOportunidades");
                });

            modelBuilder.Entity("WpOportunidades.Entities.Endereco", b =>
                {
                    b.HasOne("WpOportunidades.Entities.Oportunidade", "Oportunidade")
                        .WithOne("Endereco")
                        .HasForeignKey("WpOportunidades.Entities.Endereco", "OportunidadeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WpOportunidades.Entities.Oportunidade", b =>
                {
                    b.HasOne("WpOportunidades.Entities.OportunidadeStatus", "OportunidadeStatus")
                        .WithMany()
                        .HasForeignKey("OportunidadeStatusID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WpOportunidades.Entities.UserXOportunidade", b =>
                {
                    b.HasOne("WpOportunidades.Entities.Oportunidade", "Oportunidade")
                        .WithMany()
                        .HasForeignKey("OportunidadeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WpOportunidades.Entities.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
