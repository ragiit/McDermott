
using McDermott.Domain.Common.Interfaces;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore; 

namespace McDermott.Persistence.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
         
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            // Set CreatedBy and UpdatedBy fields before saving changes
            SetAuditFields();
            return base.SaveChanges();
        }

        private void SetAuditFields()
        {
            // Get the current user ID from the HttpContext (or a fallback if unavailable)
            var currentUserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            // Iterate through all entries to set CreatedBy or UpdatedBy
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditableEntity auditableEntity)
                {
                    // If it's a new entity, set CreatedBy and CreatedDate
                    if (entry.State == EntityState.Added)
                    {
                        auditableEntity.CreatedBy = currentUserId;
                        auditableEntity.CreatedDate = DateTime.UtcNow;
                    }

                    // If it's an updated entity, set UpdatedBy and UpdatedDate
                    if (entry.State == EntityState.Modified)
                    {
                        auditableEntity.UpdatedBy = currentUserId;
                        auditableEntity.UpdatedDate = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}