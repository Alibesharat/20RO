using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class TaxiContext : DbContext
    {
        public TaxiContext(DbContextOptions<TaxiContext> options) : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region DefaultValues

            //والدین
            modelBuilder.Entity<StudentParent>()
              .Property(C => C.BeginDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<StudentParent>()
                .Property(C => C.AvatarPath).HasDefaultValue("Default.png");
            modelBuilder.Entity<StudentParent>()
             .Property(C => C.AllowActivity).HasDefaultValue(true);
          

            //آموزشگاه
            modelBuilder.Entity<Academy>()
              .Property(C => C.BeginDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Academy>()
              .Property(C => C.AllowActivity).HasDefaultValue(false);
            modelBuilder.Entity<Academy>()
              .Property(C => C.AvatarPath).HasDefaultValue("Default.png");
            modelBuilder.Entity<Academy>()
                .Property(c => c.Longtude).HasDefaultValue("51.399735");
            modelBuilder.Entity<Academy>()
            .Property(c => c.latitude).HasDefaultValue("35.752308");

            //راننده
            modelBuilder.Entity<Driver>()
            .Property(C => C.BeginDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Driver>()
              .Property(C => C.AllowActivity).HasDefaultValue(false);
            modelBuilder.Entity<Driver>()
              .Property(C => C.AvatarPath).HasDefaultValue("Default.png");

            // تاکسی سرویس
            modelBuilder.Entity<TaxiCab>()
          .Property(C => C.DriverPercent).HasDefaultValue(80);
            #endregion

           

            #region SeedData
            //تنظیمات عمومی
            modelBuilder.Entity<GeneralSetting>()
                .HasData(new GeneralSetting() { Id = 1, SiteName = "", LogoPath = "" });
            //نقش ها
            modelBuilder.Entity<Role>()
                .HasData(
                new Role() { Id = 1, Name = "برنامه نویس" },
                new Role() { Id = 2, Name = "مدیر" },
                new Role() { Id = 3, Name = "کاربر  معمولی" }
                );
            #endregion

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Province> provinces { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<District> districts { get; set; }
        public DbSet<AcademyCategory> academyCategories { get; set; }
        public DbSet<Academy> academies { get; set; }
        public DbSet<Driver> drivers { get; set; }
        public DbSet<StudentParent> studentParents { get; set; }
        
       

        public DbSet<ServiceRequset> serviceRequsets { get; set; }
        public DbSet<TaxiCab> taxiCabs { get; set; }
        public DbSet<Pricing> pricings { get; set; }
        public DbSet<Payment> payments { get; set; }

        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<GeneralSetting> generalSettings { get; set; }
        public DbSet<DriverFactor> driverFactors { get; set; }

    }
}
