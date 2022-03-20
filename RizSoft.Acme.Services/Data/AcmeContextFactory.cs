using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizSoft.Acme.Services
{
    public class AcmeContextFactory : IDbContextFactory<AcmeContext>
    {
        private string _connectionString { get; set; }
        public AcmeContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public AcmeContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AcmeContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            return new AcmeContext(optionsBuilder.Options);
        }
    }
}
