using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Test._4_Infrastructure.Context
{
    public class MySQLContextTests : MySQLContext
    {
        public MySQLContextTests(DbContextOptions<MySQLContextTests> options) : base(options, new ConfigurationBuilder().Build()) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ignora a configuração do MySQL
            optionsBuilder.UseInMemoryDatabase("TestDatabase");
        }

    }
}
