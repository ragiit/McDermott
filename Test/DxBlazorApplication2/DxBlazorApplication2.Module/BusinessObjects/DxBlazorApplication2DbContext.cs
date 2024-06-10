using DevExpress.ExpressApp.EFCore.Updating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using System.Xml;

namespace DxBlazorApplication2.Module.BusinessObjects;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891.
public class DxBlazorApplication2ContextInitializer : DbContextTypesInfoInitializerBase
{
    protected override DbContext CreateDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DxBlazorApplication2EFCoreDbContext>()
            .UseSqlServer(";")
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
        return new DxBlazorApplication2EFCoreDbContext(optionsBuilder.Options);
    }
}

//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class DxBlazorApplication2DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DxBlazorApplication2EFCoreDbContext>
{
    public DxBlazorApplication2EFCoreDbContext CreateDbContext(string[] args)
    {
        //throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
        var optionsBuilder = new DbContextOptionsBuilder<DxBlazorApplication2EFCoreDbContext>();
        optionsBuilder.UseSqlServer("Integrated Security=SSPI;Data Source=.\\ITSSB;Initial Catalog=DxBlazorApplication3;TrustServerCertificate=True");
        optionsBuilder.UseChangeTrackingProxies();
        optionsBuilder.UseObjectSpaceLinkProxies();
        return new DxBlazorApplication2EFCoreDbContext(optionsBuilder.Options);
    }
}

[TypesInfoInitializer(typeof(DxBlazorApplication2ContextInitializer))]
public class DxBlazorApplication2EFCoreDbContext : DbContext
{
    public DxBlazorApplication2EFCoreDbContext(DbContextOptions<DxBlazorApplication2EFCoreDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<DemoTask> DemoTasks { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationUserLoginInfo> UserLoginInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Menetapkan strategi pelacakan perubahan untuk entitas tertentu
        modelBuilder.Entity<Department>()
            .HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);

        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        modelBuilder.UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
        base.OnModelCreating(modelBuilder);
    }
}