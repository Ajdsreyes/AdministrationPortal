using AdministrationPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace AdministrationPortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tables registered for the database
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<History> Histories { get; set; }

        // ADD THIS: This handles the labels, field types, and options for each service
        public DbSet<ServicePurpose> ServicePurposes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This ensures EF doesn't get confused by singular/plural naming
            modelBuilder.Entity<Status>().ToTable("Statuses");
            modelBuilder.Entity<History>().ToTable("Histories");

            // This tells the database how to handle the new columns
            modelBuilder.Entity<ServicePurpose>(entity =>
            {
                entity.Property(e => e.FieldType).HasMaxLength(50);
                entity.Property(e => e.IsRequired).HasDefaultValue(true);
                // Options can store long strings for dropdown values
                entity.Property(e => e.Options).IsRequired(false);
            });

            // If your SQL script uses decimal(18,2) for income, define it here to avoid warnings
            modelBuilder.Entity<ServiceRequest>()
                .Property(r => r.GrossAnnualIncome)
                .HasPrecision(18, 2);
        }
    }
}