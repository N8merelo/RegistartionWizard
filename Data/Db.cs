using Microsoft.EntityFrameworkCore;
using RegistrationWizard.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RegistartionWizard.Data
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TermsAcceptance> TermsAcceptances { get; set; }
        public DbSet<Industry> Industries { get; set; }
    }
}