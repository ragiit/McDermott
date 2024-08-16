using McHealthCare.Domain.Common;
using McHealthCare.Domain.Entities;
using McHealthCare.Domain.Entities.ClinicService;
using McHealthCare.Domain.Entities.Configuration;
using McHealthCare.Domain.Entities.Employees;
using McHealthCare.Domain.Entities.Inventory;
using McHealthCare.Domain.Entities.Medical;
using McHealthCare.Domain.Entities.Pharmacies;
using McHealthCare.Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace McHealthCare.Context
{
    public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor? httpContextAccessor = null) : IdentityDbContext<ApplicationUser>(options)
    {
        #region DbSet

        #region BPJS Integration (Kalo ada relasi di buat Set To Null)

        public DbSet<Awareness> Awarenesses { get; set; }
        public DbSet<Allergy> Allergies { get; set; }

        #endregion BPJS Integration (Kalo ada relasi di buat Set To Null)

        #region BPJS

        public DbSet<BpjsClassification> BpjsClassifications { get; set; }
        public DbSet<BPJSIntegration> BPJSIntegrations { get; set; }
        public DbSet<SystemParameter> SystemParameters { get; set; }

        #endregion BPJS

        #region Config

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Religion> Religions { get; set; }
        public DbSet<Village> Villages { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Occupational> Occupationals { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMenu> GroupMenus { get; set; }
        public DbSet<EmailSetting> EmailSettings { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        #endregion Config

        #region Medical

        public DbSet<DiseaseCategory> DiseaseCategories { get; set; }
        public DbSet<NursingDiagnoses> NursingDiagnoses { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<ChronicCategory> CronisCategories { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<HealthCenter> HealthCenters { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<BuildingLocation> BuildingLocations { get; set; }
        public DbSet<LabTestDetail> LabTestDetails { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabUom> LabUoms { get; set; }
        public DbSet<SampleType> SampleTypes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        #endregion Medical

        #region Patiente

        public DbSet<Family> Families { get; set; }
        public DbSet<InsurancePolicy> InsurancePolicies { get; set; }
        public DbSet<PatientFamilyRelation> PatientFamilyRelations { get; set; }
        public DbSet<PatientAllergy> PatientAllergies { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<DoctorScheduleDetail> DoctorScheduleDetails { get; set; }
        public DbSet<DoctorScheduleSlot> DoctorScheduleSlots { get; set; }
        public DbSet<ClassType> ClassTypes { get; set; }

        #endregion Patiente

        #region Transfer

        public DbSet<Accident> Accidents { get; set; }
        public DbSet<LabResultDetail> LabResultDetails { get; set; }
        public DbSet<GeneralConsultanService> GeneralConsultanServices { get; set; }
        public DbSet<GeneralConsultanCPPT> GeneralConsultanCPPTs { get; set; }
        public DbSet<GeneralConsultanMedicalSupport> GeneralConsultanMedicalSupports { get; set; }

        public DbSet<GeneralConsultationLog> GeneralConsultationLogs { get; set; }

        #endregion Transfer

        #region Queue

        public DbSet<KioskConfig> KioskConfigs { get; set; }
        public DbSet<KioskQueue> KioskQueues { get; set; }
        public DbSet<Kiosk> Kiosks { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<QueueDisplay> QueueDisplays { get; set; }
        public DbSet<DetailQueueDisplay> DetailQueueDisplays { get; set; }

        #endregion Queue

        #region Pharmacy

        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Concoction> Concoctions { get; set; }
        public DbSet<ConcoctionLine> ConcoctionLines { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<DrugRoute> DrugRoutes { get; set; }
        public DbSet<DrugDosage> DrugDosages { get; set; }
        public DbSet<Signa> Signas { get; set; }
        public DbSet<ActiveComponent> ActiveComponents { get; set; }
        public DbSet<UomCategory> UomCategories { get; set; }
        public DbSet<Uom> Uoms { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<DrugForm> FormDrugs { get; set; }
        public DbSet<MedicamentGroup> MedicamentGroups { get; set; }
        public DbSet<MedicamentGroupDetail> MedicamentGroupDetails { get; set; }
        public DbSet<ReorderingRule> ReorderingRules { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<StockProduct> StockProducts { get; set; }
        public DbSet<ReceivingStockProduct> ReceivingStockDetails { get; set; }
        public DbSet<ReceivingStock> ReceivingStocks { get; set; }
        public DbSet<ReceivingLog> ReceivingLogs { get; set; }
        public DbSet<TransferStock> TransferStocks { get; set; }
        public DbSet<TransferStockLog> TransferStockLogs { get; set; }
        public DbSet<PharmacyLog> PharmacyLogs { get; set; }
        public DbSet<SickLeave> SickLeaves { get; set; }
        public DbSet<StockOutPrescription> StockOutPrescriptions { get; set; }
        public DbSet<StockOutLines> StockOutLines { get; set; }
        public DbSet<TransactionStock> TransactionStocks { get; set; }

        #endregion Pharmacy

        #region MyRegion

        public DbSet<InventoryAdjusment> InventoryAdjusments { get; set; }
        public DbSet<InventoryAdjusmentDetail> InventoryAdjusmentDetails { get; set; }

        #endregion MyRegion

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

            var userId = httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? Guid.Empty.ToString();

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