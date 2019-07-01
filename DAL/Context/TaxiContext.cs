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

            //ادمین
            modelBuilder.Entity<Admin>()
            .Property(C => C.AllowActivity).HasDefaultValue(true);
            modelBuilder.Entity<Admin>()
           .Property(C => C.Id).HasDefaultValue(Const.Generatetoken());


            //والدین
            modelBuilder.Entity<StudentParent>()
              .Property(C => C.BeginDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Driver>()
                .Property(C => C.AvatarPath).HasDefaultValue("Default.png");
            modelBuilder.Entity<Driver>()
              .Property(C => C.Token).HasDefaultValue(Const.Generatetoken());
            modelBuilder.Entity<StudentParent>()
             .Property(C => C.AllowActivity).HasDefaultValue(true);


            //مدرسه
            modelBuilder.Entity<Academy>()
              .Property(C => C.BeginDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Academy>()
              .Property(C => C.AllowActivity).HasDefaultValue(false);
            modelBuilder.Entity<Academy>()
              .Property(C => C.AvatarPath).HasDefaultValue("Default.png");
            modelBuilder.Entity<Academy>()
                .Property(c => c.Longtude).HasDefaultValue("51.399735");
            modelBuilder.Entity<Academy>()
            .Property(c => c.Latitude).HasDefaultValue("35.752308");
            modelBuilder.Entity<Academy>()
              .Property(C => C.Token).HasDefaultValue(Const.Generatetoken());
            modelBuilder.Entity<Academy>()
             .Property(c => c.AcademyPercent).HasDefaultValue(1);

            //راننده
            modelBuilder.Entity<Driver>()
            .Property(C => C.BeginDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Driver>()
              .Property(C => C.AllowActivity).HasDefaultValue(false);
            modelBuilder.Entity<Driver>()
              .Property(C => C.AvatarPath).HasDefaultValue("Default.png");
            modelBuilder.Entity<Driver>()
              .Property(C => C.Token).HasDefaultValue(Const.Generatetoken());


            //پیمانکار
            modelBuilder.Entity<Contractor>()
            .Property(C => C.BeginDate).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Contractor>()
              .Property(C => C.AllowActivity).HasDefaultValue(true);
            modelBuilder.Entity<Contractor>()
              .Property(C => C.AvatarPath).HasDefaultValue("Default.png");
            modelBuilder.Entity<Contractor>()
              .Property(C => C.Token).HasDefaultValue(Const.Generatetoken());

            // تاکسی سرویس
            modelBuilder.Entity<TaxiService>()
          .Property(C => C.DriverPercent).HasDefaultValue(75)/* سهم پیش فرض راننده 75درصد می باشد*/;
            modelBuilder.Entity<TaxiService>()
                .Property(c => c.Id).HasDefaultValue(Const.Generatetoken());

            //درخواست سرویس
            modelBuilder.Entity<ServiceRequset>()
               .Property(c => c.Id).HasDefaultValue(Const.Generatetoken());

            //ضریب شهر
            modelBuilder.Entity<City>()
                .Property(c => c.CityPercent).HasDefaultValue(1);

            //ضریب منطقه
            modelBuilder.Entity<District>()
              .Property(c => c.DistrictPercent).HasDefaultValue(1);
            #endregion


            #region SeedData
            //تنظیمات عمومی
            modelBuilder.Entity<GeneralSetting>()
                .HasData(new GeneralSetting() { Id = 1, SiteName = "", LogoPath = "", TaxiPercent = 2500, VanPercent = 2000 });
            //نقش ها
            modelBuilder.Entity<Role>()
                .HasData(
                new Role() { Id = 1, Name = "برنامه نویس" },
                new Role() { Id = 2, Name = "مدیر" },
                new Role() { Id = 3, Name = "کاربر  معمولی" }
                );

            modelBuilder.Entity<Admin>()
               .HasData(
               new Admin() {Id=Const.Generatetoken(), Name = "Admin", Password = "123456789", Username = "09195410188" });
           
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<AcademyCategory> AcademyCategories { get; set; }
        public DbSet<Academy> Academies { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<StudentParent> StudentParents { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<ServiceRequset> ServiceRequsets { get; set; }
        public DbSet<TaxiService> TaxiServices { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }
        public DbSet<Accounting> Accountings { get; set; }

    }
}
