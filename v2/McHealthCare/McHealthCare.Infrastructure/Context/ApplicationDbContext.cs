using McHealthCare.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace McHealthCare.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        #region DbSet

        #region Configuratioin

        public DbSet<Country> Countries { get; set; }

        #endregion Configuratioin

        #endregion DbSet
    }
}