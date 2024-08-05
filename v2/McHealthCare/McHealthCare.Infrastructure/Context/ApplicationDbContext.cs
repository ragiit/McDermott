using McHealthCare.Domain.Common;
using McHealthCare.Domain.Entities;
using McHealthCare.Domain.Entities.Configuration;
using McHealthCare.Domain.Entities.Medical;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace McHealthCare.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<ApplicationUser>(options)
    {
        #region DbSet

        #region Configuration

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Village> Villages { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMenu> GroupMenus { get; set; }

        #endregion Configuration

        public DbSet<Building> Buildings { get; set; }
        public DbSet<BuildingLocation> BuildingLocations { get; set; }
        public DbSet<ChronicCategory> ChronicCategories { get; set; }
        public DbSet<Diagnosis> Diagnosis { get; set; }
        public DbSet<DiseaseCategory> DiseaseCategories { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<DoctorScheduleDetail> DoctorScheduleDetails { get; set; }
        public DbSet<DoctorScheduleSlot> DoctorScheduleSlots { get; set; }
        public DbSet<HealthCenter> HealthCenters { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabTestDetail> LabTestDetails { get; set; }
        public DbSet<LabUom> LabUoms { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<NursingDiagnoses> NursingDiagnoses { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<SampleType> SampleTypes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Specialist> Specialists { get; set; }

        #endregion DbSet

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Group>()
                 .HasMany(m => m.GroupMenus)
                 .WithOne(c => c.Group)
                 .OnDelete(DeleteBehavior.Cascade);
        }

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