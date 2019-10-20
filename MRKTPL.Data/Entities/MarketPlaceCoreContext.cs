using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MRKTPL.Data.Entities
{
    public partial class MarketPlaceCoreContext : DbContext
    {
        #region CTor
        public MarketPlaceCoreContext()
        {

        }
        public MarketPlaceCoreContext(DbContextOptions<MarketPlaceCoreContext> options)
            : base(options)
        { }
        #endregion
        #region Methods


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                string connectionString = configuration.GetConnectionString("DatabaseConnStr");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.Property(e => e.UserID)
                   .HasMaxLength(200)
                   .IsUnicode(false);
                //entity.Property(e => e.UserName)
                //   .HasMaxLength(200)
                //   .IsUnicode(false);

                //entity.Property(e => e.FirstName)
                //    .HasMaxLength(200)
                //    .IsUnicode(false);
                //entity.Property(e => e.LastName)
                //   .HasMaxLength(200)
                //   .IsUnicode(false);

                //entity.Property(e => e.Email)
                //    .HasMaxLength(128)
                //    .IsUnicode(false);

                //entity.Property(e => e.MobileNo)
                //    .HasMaxLength(12)
                //    .IsUnicode(false);

                //entity.Property(e => e.Password)
                //    .HasMaxLength(20)
                //    .IsUnicode(false);
                //entity.Property(e => e.RoleID)
                //  .HasMaxLength(20)
                //  .IsUnicode(false);
                //entity.Property(e => e.PasswordHash)
                //  .HasMaxLength(20)
                //  .IsUnicode(false);
                //entity.Property(e => e.PasswordSalt)
                //  .HasMaxLength(20)
                //  .IsUnicode(false);
                //entity.Property(e => e.IsActive)
                //  .HasMaxLength(20)
                //  .IsUnicode(false);
                //entity.Property(e => e.CreatedBy)
                //  .HasMaxLength(20)
                //  .IsUnicode(false);
                //entity.Property(e => e.ModifiedBy)
                //  .HasMaxLength(20)
                //  .IsUnicode(false);
                //entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                //entity.Property(e => e.ModifiedDate).HasColumnType("datetime");


            });
            modelBuilder.Entity<CategoryMaster>(entity =>
            {
                entity.Property(e => e.CategoryID)
                  .HasMaxLength(200)
                  .IsUnicode(false);
                //entity.Property(e => e.CategoryTitle)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);
                //entity.Property(e => e.ParentCategoryShortDesc)
                //    .HasMaxLength(500)
                //    .IsUnicode(false);
                //entity.Property(e => e.ParentCategoryLongDesc)
                //   .HasMaxLength(500)
                //   .IsUnicode(false);
                //entity.Property(e => e.ParentCategoryTitle)
                //   .HasMaxLength(500)
                //   .IsUnicode(false);
                //entity.Property(e => e.IsParentCategory)
                //   .HasMaxLength(500)
                //   .IsUnicode(false);
                //entity.Property(e => e.IsSubCategory)
                //   .HasMaxLength(500)
                //   .IsUnicode(false);

                //entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                //entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                //entity.HasOne(d => d.IsParentCategory)
                //    .WithMany(p => p.InverseParentCategory)
                //    .HasForeignKey(d => d.ParentCategoryId)
                //    .HasConstraintName("FK_Category_Category1");
            });
            modelBuilder.Entity<ProductMaster>(entity =>
            {
                entity.Property(e => e.ProductID)
                  .HasMaxLength(200)
                  .IsUnicode(false);
            });
            modelBuilder.Entity<ProductCategoryMapping>(entity =>
            {
                entity.Property(e => e.Id)
                  .HasMaxLength(200)
                  .IsUnicode(false);
            });
            modelBuilder.Entity<RoleMaster>(entity =>
            {
                entity.Property(e => e.RoleID)
                  .HasMaxLength(200)
                  .IsUnicode(false);
            });
            modelBuilder.Entity<RoleRightsMaster>(entity =>
            {
                entity.Property(e => e.RoleRightsMappingID)
                  .HasMaxLength(200)
                  .IsUnicode(false);
            });
        }
        public virtual DbSet<UserMaster> Users { get; set; }
        public virtual DbSet<RoleMaster> Roles { get; set; }
        public virtual DbSet<RoleRightsMaster> RoleRights { get; set; }
        
        public virtual DbSet<CategoryMaster> Category { get; set; }

        public virtual DbSet<ProductMaster> Products { get; set; }
        public virtual DbSet<ProductCategoryMapping> ProductCategoryMapping { get; set; }
        
        #endregion
    }
}


