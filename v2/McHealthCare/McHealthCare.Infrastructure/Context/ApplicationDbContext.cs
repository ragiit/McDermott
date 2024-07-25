using McHealthCare.Domain.Common;
using McHealthCare.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace McHealthCare.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
    {
        #region DbSet

        #region Configuratioin

        public DbSet<Country> Countries { get; set; }

        #endregion Configuratioin

        #endregion DbSet 

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                        .Entries()
                        .Where(e => e.Entity is BaseAuditableEntity &&
                                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString();

            foreach (var entry in entries)
            {
                var auditableEntity = (BaseAuditableEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    auditableEntity.CreatedBy = Guid.Parse(userId);
                    auditableEntity.CreatedDate = DateTime.UtcNow;
                }
                else
                {
                    auditableEntity.UpdatedBy = Guid.Parse(userId);
                    auditableEntity.UpdatedDate = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}