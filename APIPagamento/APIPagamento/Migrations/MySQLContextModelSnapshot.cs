﻿// <auto-generated />
using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APIPagamento.Migrations
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [DbContext(typeof(MySQLContext))]
    partial class MySQLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Pagamento", b =>
                {
                    b.Property<int>("IdPagamento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdPagamento"));

                    b.Property<DateTime>("DataPagamento")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdPedido")
                        .HasColumnType("int");

                    b.Property<string>("MetodoPagamento")
                        .HasColumnType("longtext");

                    b.Property<string>("StatusPagamento")
                        .HasColumnType("longtext");

                    b.Property<float>("ValorPagamento")
                        .HasColumnType("float");

                    b.HasKey("IdPagamento");

                    b.ToTable("Pagamento");
                });
#pragma warning restore 612, 618
        }
    }
}
