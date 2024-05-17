using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class MySQLContext(DbContextOptions options, IConfiguration configuration) : DbContext(options)
    {
        private readonly IConfiguration _configuration = configuration;

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringMysql = _configuration.GetConnectionString("ConnectionMysql");
            optionsBuilder.UseMySql(connectionStringMysql, ServerVersion.AutoDetect(connectionStringMysql), builder => builder.MigrationsAssembly("APIPagamento"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração das entidades do modelo, incluindo chaves primárias, chaves estrangeiras e outros relacionamentos.

            //modelBuilder.Entity<Pedido>().HasKey(p => p.IdPedido);

            modelBuilder.Entity<Pagamento>().HasKey(p => p.IdPagamento);

            //Em resumo, esse trecho de código está configurando um relacionamento de "tem um"
            //entre as entidades Pagamento e Pedido, onde um pagamento está associado a um único pedido.
            //A chave estrangeira IdPedido na tabela de Pagamento será usada para referenciar a tabela de Pedido.
            //modelBuilder.Entity<Pagamento>()
            //            .HasOne(p => p.Pedido)
            //            .WithMany()
            //            .HasForeignKey(p => p.IdPedido);
        }

        public DbSet<Pagamento>? Pagamento { get; set; }
        //public DbSet<Pedido>? Pedido { get; set; }
    }
}
