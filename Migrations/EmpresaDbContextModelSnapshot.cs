﻿// <auto-generated />
using System;
using API_sis_conselhotutelarv2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API_sis_conselhotutelarv2.Migrations
{
    [DbContext(typeof(EmpresaDbContext))]
    partial class EmpresaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("API_sis_conselhotutelarv2.Models.ChaveValidade", b =>
                {
                    b.Property<int>("Cha_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Cha_Chave")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("varchar(8)");

                    b.Property<int>("Cha_IdEmpresa")
                        .HasColumnType("int");

                    b.Property<DateTime>("Cha_Validade")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Cha_Id");

                    b.HasIndex("Cha_Chave")
                        .IsUnique();

                    b.HasIndex("Cha_IdEmpresa");

                    b.ToTable("ChavesValidade");
                });

            modelBuilder.Entity("API_sis_conselhotutelarv2.Models.Empresa", b =>
                {
                    b.Property<int>("Emp_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Ativo_Inativo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Emp_CNPJ")
                        .HasMaxLength(14)
                        .HasColumnType("varchar(14)");

                    b.Property<string>("Emp_Connection")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Emp_RazaoSocial")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Emp_Telefone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Emp_Id");

                    b.HasIndex("Emp_CNPJ")
                        .IsUnique();

                    b.ToTable("ChaveValidade", (string)null);
                });

            modelBuilder.Entity("API_sis_conselhotutelarv2.Models.ChaveValidade", b =>
                {
                    b.HasOne("API_sis_conselhotutelarv2.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("Cha_IdEmpresa")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Empresa");
                });
#pragma warning restore 612, 618
        }
    }
}
