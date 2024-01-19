using McDermott.Domain.Common;
using McDermott.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace McDermott.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        //public DbSet<Stadium> Stadiums => Set<Stadium>();

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Gender> Genders { get; set; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ---> sintaks ini sangat diperlukan bila menggunakan Identity, kalau dirumah update-database selalu error

            // define DeleteBehaviour
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

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

            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            modelBuilder.Entity<City>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<Country>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<Gender>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<Group>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<Menu>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<Province>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<Religion>().HasIndex(u => u.Name).IsUnique();
            modelBuilder.Entity<Village>().HasIndex(u => u.Name).IsUnique();

            // apply configuration dengan membaca assembly
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // seed data
            //SeedIdentity(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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
            return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
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
    }
}