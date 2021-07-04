using ContactInformationCore.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactInformationCore.WebAPI
{
    public class DatabaseContaxt : DbContext
    {
        public DatabaseContaxt(DbContextOptions<DatabaseContaxt> options) : base(options) { }

        public DbSet<ContactInformation> Contacts { get; set; }
    }
}
