using McDermott.Domain.Common;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using System.Security.Claims;

namespace McDermott.Persistence.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        #region DbSet

        #region BPJS Integration (Kalo ada relasi di buat Set To Null)

        public DbSet<Awareness> Awarenesses { get; set; }
        public DbSet<Allergy> Allergies { get; set; }

        #endregion BPJS Integration (Kalo ada relasi di buat Set To Null)

        #region BPJS

        public DbSet<BpjsClassification> BpjsClassifications { get; set; }

        //public DbSet<BPJSIntegration> BPJSIntegrations { get; set; }
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
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Occupational> Occupationals { get; set; }
        public DbSet<User> Users { get; set; }
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
        public DbSet<CronisCategory> CronisCategories { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<HealthCenter> HealthCenters { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<BuildingLocation> BuildingLocations { get; set; }
        public DbSet<LabTestDetail> LabTestDetails { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabUom> LabUoms { get; set; }
        public DbSet<SampleType> SampleTypes { get; set; }
        public DbSet<Project> Projects { get; set; }

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
        public DbSet<GeneralConsultationLog> GeneralConsultanServiceLogs { get; set; }
        public DbSet<GeneralConsultantClinicalAssesment> GeneralConsultantClinicalAssesments { get; set; }
        public DbSet<GeneralConsultanCPPT> GeneralConsultanCPPTs { get; set; }
        public DbSet<GeneralConsultanMedicalSupport> GeneralConsultanMedicalSupports { get; set; }

        public DbSet<GeneralConsultanMedicalSupportLog> GeneralConsultanMedicalSupportLogs { get; set; }
        public DbSet<VaccinationPlan> VaccinationPlans { get; set; }
        public DbSet<GeneralConsultanServiceAnc> GeneralConsultanServiceAncs { get; set; }
        public DbSet<GeneralConsultanServiceAncDetail> GeneralConsultanServiceAncDetails { get; set; }

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
        public DbSet<GoodsReceiptDetail> GoodsReceiptDetails { get; set; }
        public DbSet<GoodsReceipt> GoodsReceipts { get; set; }
        public DbSet<GoodsReceiptLog> GoodsReceiptLogs { get; set; }
        public DbSet<TransferStock> TransferStocks { get; set; }
        public DbSet<TransferStockLog> TransferStockLogs { get; set; }
        public DbSet<PharmacyLog> PharmacyLogs { get; set; }
        public DbSet<SickLeave> SickLeaves { get; set; }
        public DbSet<StockOutPrescription> StockOutPrescriptions { get; set; }
        public DbSet<StockOutLines> StockOutLines { get; set; }
        public DbSet<TransactionStock> TransactionStocks { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public DbSet<MaintenanceProduct> MaintenanceProducts { get; set; }

        #endregion Pharmacy

        #region MyRegion

        public DbSet<WellnessProgram> WellnessPrograms { get; set; }
        public DbSet<WellnessProgramAttendance> WellnessProgramAttendances { get; set; }
        public DbSet<WellnessProgramSession> WellnessProgramSessions { get; set; }
        public DbSet<GeneralConsultationServiceLog> GeneralConsultationServiceLogs { get; set; }
        public DbSet<InventoryAdjusment> InventoryAdjusments { get; set; }
        public DbSet<InventoryAdjusmentDetail> InventoryAdjusmentDetails { get; set; }
        public DbSet<InventoryAdjusmentLog> InventoryAdjusmentLogs { get; set; }

        #endregion MyRegion

        #endregion DbSet

        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Definisikan indeks untuk kolom Name
            modelBuilder.Entity<Village>()
                .HasIndex(v => v.Name)
                .HasDatabaseName("IX_Villages_Name");
            modelBuilder.Entity<City>()
               .HasIndex(v => v.Name)
               .HasDatabaseName("IX_Cities_Name");
            modelBuilder.Entity<Province>()
               .HasIndex(v => v.Name)
               .HasDatabaseName("IX_Provinces_Name");
            modelBuilder.Entity<District>()
               .HasIndex(v => v.Name)
               .HasDatabaseName("IX_Districts_Name");
            modelBuilder.Entity<Menu>()
               .HasIndex(v => v.Name)
               .HasDatabaseName("IX_Menus_Name");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<User>()
                   .HasIndex(e => e.NIP)
                   .IsUnique();

            modelBuilder.Entity<User>()
                  .HasIndex(e => e.Oracle)
                  .IsUnique();

            modelBuilder.Entity<User>()
                  .HasIndex(e => e.SAP)
                  .IsUnique();

            modelBuilder.Entity<User>()
                  .HasIndex(e => e.Legacy)
                  .IsUnique();

            modelBuilder.Entity<InsurancePolicy>()
                  .HasIndex(e => e.NoKartu)
                  .IsUnique();

            modelBuilder.Entity<GoodsReceiptDetail>()
              .HasOne(h => h.Product)
              .WithMany(m => m.GoodsReceiptDetail)
              .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<GoodsReceiptDetail>()
              .HasOne(h => h.Stock)
              .WithMany(m => m.GoodsReceiptDetail)
              .OnDelete(DeleteBehavior.SetNull);

            // Contoh: Aturan cascade delete untuk hubungan many-to-many

            // Menentukan indeks menggunakan Fluent API
            modelBuilder.Entity<StockOutPrescription>()
                .HasOne(h => h.Prescription)
                .WithMany(x => x.StockOutPrescription)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StockOutPrescription>()
                .HasOne(h => h.TransactionStock)
                .WithMany(x => x.StockOutPrescription)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StockOutLines>()
                .HasOne(h => h.TransactionStock)
                .WithMany(x => x.StockOutLines)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TransferStockProduct>()
                .HasOne(h => h.TransferStock)
                .WithMany(x => x.TransferStockProduct)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<GeneralConsultantClinicalAssesment>()
               .HasOne(h => h.Awareness)
               .WithMany(x => x.GeneralConsultantClinicalAssesments)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TransferStockLog>()
                .HasOne(h => h.TransferStock)
                .WithMany(x => x.TransferStockLog)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TransferStockLog>()
                .HasOne(h => h.Source)
                .WithMany(x => x.TransferStockLog)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Medicament>()
                .HasOne(e => e.Product)
                .WithMany(m => m.Medicaments)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Medicament>()
                .HasOne(e => e.Route)
                .WithMany(m => m.Medicaments)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Medicament>()
                .HasOne(e => e.Frequency)
                .WithMany(m => m.Medicaments)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Medicament>()
                .HasOne(e => e.Uom)
                .WithMany(m => m.Medicaments)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<LabTest>()
               .HasOne(e => e.SampleType)
               .WithMany(m => m.LabTests)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Locations>()
            .HasOne(e => e.ParentLocation)
            .WithMany()
            .HasForeignKey(e => e.ParentLocationId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Village>()
                .HasIndex(p => p.Id)
                .IsUnique();

            modelBuilder.Entity<DrugRoute>()
                 .HasMany(m => m.DrugDosages)
                 .WithOne(c => c.DrugRoute)
                 .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Locations>()
                .HasMany(m => m.ReorderingRules)
                .WithOne(c => c.Location)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Company>()
                .HasMany(m => m.ReorderingRules)
                .WithOne(c => c.Company)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Company>()
               .HasMany(m => m.Locations)
               .WithOne(c => c.Company)
               .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Uom>()
                  .HasMany(m => m.ActiveComponents)
                  .WithOne(c => c.Uom)
                  .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<UomCategory>()
                  .HasMany(m => m.Uoms)
                  .WithOne(c => c.UomCategory)
                  .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<GeneralConsultanService>()
                  .HasMany(m => m.GeneralConsultanCPPTs)
                  .WithOne(c => c.GeneralConsultanService)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GeneralConsultanService>()
                  .HasMany(m => m.GeneralConsultanCPPTs)
                  .WithOne(c => c.GeneralConsultanService)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GeneralConsultanService>()
                  .HasMany(m => m.GeneralConsultationLogs)
                  .WithOne(c => c.GeneralConsultanService)
                  .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Family>()
            //      .HasOne(m => m.InverseRelation)
            //      .WithMany() // This indicates the inverse can have many related entities
            //      .HasForeignKey(m => m.InverseRelationId)
            //      .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GeneralConsultanService>()
                .HasMany(m => m.Accidents)
                .WithOne(c => c.GeneralConsultanService)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GeneralConsultanService>()
                .HasMany(m => m.SickLeaves)
                .WithOne(c => c.GeneralConsultans)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryAdjusment>()
              .HasMany(m => m.InventoryAdjusmentDetails)
              .WithOne(c => c.InventoryAdjusment)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                  .HasMany(m => m.GroupMenus)
                  .WithOne(c => c.Group)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pharmacy>()
                 .HasMany(m => m.Prescriptions)
                 .WithOne(c => c.Pharmacy)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LabTest>()
                  .HasMany(m => m.LabTestDetails)
                  .WithOne(c => c.LabTest)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LabTest>()
                .HasMany(m => m.GeneralConsultanMedicalSupports)
                .WithOne(c => c.LabTest)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Menu>()
                  .HasMany(m => m.GroupMenus)
                  .WithOne(c => c.Menu)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GeneralConsultanService>()
                  .HasMany(m => m.GeneralConsultanMedicalSupports)
                  .WithOne(c => c.GeneralConsultanService)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GeneralConsultanService>()
                  .HasMany(m => m.GeneralConsultantClinicalAssesments)
                  .WithOne(c => c.GeneralConsultanService)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GeneralConsultanMedicalSupport>()
                  .HasMany(m => m.LabResultDetails)
                  .WithOne(c => c.GeneralConsultanMedicalSupport)
                  .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Department>()
                .HasOne(e => e.Manager)
                .WithMany();

            modelBuilder.Entity<User>()
                .HasOne(e => e.Department)
                .WithMany();

            modelBuilder.Entity<User>()
             .HasMany(m => m.PatientAllergies)
             .WithOne(c => c.User)
             .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<User>()
            // .HasMany(m => m.PatientFamilyRelations)
            // .WithOne(c => c.Patient)
            // .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<User>()
            // .HasMany(m => m.PatientFamilyRelations)
            // .WithOne(c => c.FamilyMember)
            // .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<User>()
            //    .HasMany(m => m.PatientAllergies)
            //    .WithOne(c => c.User)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<QueueDisplay>()
            //    .HasMany(m => m.Counter)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Province>()
            //  .HasMany(m => m.Districts)
            //  .WithOne(c => c.Province)
            //  .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Province>()
            //.HasMany(m => m.Cities)
            //.WithOne(c => c.Province)
            //.OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Province>()
            //.HasMany(m => m.Companies)
            //.WithOne(c => c.Province)
            //.OnDelete(DeleteBehavior.Cascade);

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            //var entityTypes = modelBuilder.Model
            //    .GetEntityTypes()
            //    .ToList();

            //var foreignKeys = entityTypes
            //.SelectMany(e => e.GetForeignKeys().Where(f => f.DeleteBehavior == DeleteBehavior.Cascade));

            //foreach (var foreignKey in foreignKeys)
            //{
            //    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            //}

            //modelBuilder.Entity<Menu>().HasData(new Menu
            //{
            //    Id = 1,
            //    Name = "Configuration",
            //    Sequence = "1"
            //}, new
            //{
            //    Id = 2,
            //    Name = "User",
            //    Sequence = "1",
            //    Url = "users",
            //    ParentMenu = "Configuration"
            //}, new
            //{
            //    Id = 3,
            //    Name = "Group",
            //    Sequence = "2",
            //    Url = "groups",
            //    ParentMenu = "Configuration"
            //},
            //new
            //{
            //    Id = 4,
            //    Name = "Menu",
            //    Sequence = "3",
            //    Url = "menus",
            //    ParentMenu = "Configuration"
            //},
            //new
            //{
            //    Id = 5,
            //    Name = "Email Setting",
            //    Sequence = "4",
            //    Url = "email-settings",
            //    ParentMenu = "Configuration"
            //}, new
            //{
            //    Id = 6,
            //    Name = "Email Template",
            //    Sequence = "5",
            //    Url = "email-template",
            //    ParentMenu = "Configuration"
            //}, new
            //{
            //    Id = 7,
            //    Name = "Company",
            //    Sequence = "6",
            //    Url = "companies",
            //    ParentMenu = "Configuration"
            //}, new
            //{
            //    Id = 8,
            //    Name = "Country",
            //    Sequence = "7",
            //    Url = "countries",
            //    ParentMenu = "Configuration"
            //});

            //modelBuilder.Entity<Group>().HasData(new Group
            //{
            //    Id = 1,
            //    Name = "Admin"
            //});

            //modelBuilder.Entity<GroupMenu>().HasData(new GroupMenu
            //{
            //    Id = 1,
            //    GroupId = 1,
            //    MenuId = 1
            //},
            //new
            //{
            //    Id = 2,
            //    GroupId = 1,
            //    MenuId = 2
            //},
            //new
            //{
            //    Id = 3,
            //    GroupId = 1,
            //    MenuId = 3
            //},
            //new
            //{
            //    Id = 4,
            //    GroupId = 1,
            //    MenuId = 4
            //},
            //new
            //{
            //    Id = 5,
            //    GroupId = 1,
            //    MenuId = 5
            //},
            //new
            //{
            //    Id = 6,
            //    GroupId = 1,
            //    MenuId = 6
            //},
            //new
            //{
            //    Id = 7,
            //    GroupId = 1,
            //    MenuId = 7
            //},
            //new
            //{
            //    Id = 8,
            //    GroupId = 1,
            //    MenuId = 8
            //});

            //modelBuilder.Entity<User>().HasData(new User
            //{
            //    Id = 1,
            //    Name = "Admin",
            //    Email = "admin@example.com",
            //    Password = "123",
            //    GroupId = 1
            //});

            //modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            //modelBuilder.Entity<City>().HasIndex(u => u.Name).IsUnique();
            //modelBuilder.Entity<Country>().HasIndex(u => u.Name).IsUnique();
            //modelBuilder.Entity<Gender>().HasIndex(u => u.Name).IsUnique();
            //modelBuilder.Entity<Group>().HasIndex(u => u.Name).IsUnique();
            //modelBuilder.Entity<Menu>().HasIndex(u => u.Name).IsUnique();
            //modelBuilder.Entity<Province>().HasIndex(u => u.Name).IsUnique();
            //modelBuilder.Entity<Religion>().HasIndex(u => u.Name).IsUnique();
            //modelBuilder.Entity<Village>().HasIndex(u => u.Name).IsUnique();

            // apply configuration dengan membaca assembly
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // seed data
            //SeedIdentity(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var currentUser = _httpContextAccessor is null ? string.Empty : _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);

                var entries = ChangeTracker.Entries<BaseAuditableEntity>();

                foreach (var entry in entries)
                {
                    var now = DateTime.Now;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedBy = currentUser;
                            entry.Entity.CreatedDate = now;
                            break;

                        case EntityState.Modified:
                            entry.Entity.UpdatedBy = currentUser;
                            entry.Entity.UpdatedDate = now;
                            break;
                    }
                }

                return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error(
                 "\n\n" +
                 "==================== SAVE ASYNC ERROR ====================" + "\n" +
                 "Message =====> An error occurred while saving data: " + ex.Message + "\n" +
                 "Inner Message =====> An error occurred while saving data: " + ex.InnerException?.Message + "\n" +
                 "Stack Trace =====> " + ex.StackTrace?.Trim() + "\n" +
                 "==================== SAVE ASYNC ERROR ====================" + "\n"
                 );
                throw;
            }
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            //foreach (var entry in entries)
            //{
            //    var user = Helper.UserLogin;
            //    var now = DateTime.Now;

            //    switch (entry.State)
            //    {
            //        case EntityState.Added:
            //            entry.Entity.CreatedBy = user.Name;
            //            entry.Entity.CreatedOn = now;
            //            break;

            //        case EntityState.Modified:
            //            entry.Entity.LastUpdatedBy = user.Name;
            //            entry.Entity.LastUpdatedOn = now;
            //            break;
            //    }
            //}
            return SaveChangesAsync().GetAwaiter().GetResult();
        }

        #endregion OnModelCreating
    }
}