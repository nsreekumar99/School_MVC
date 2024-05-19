using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Models;
using School.Models.Models;

namespace School.DataAccess.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Qualifications> Qualifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Qualifications>(entity =>
            {
                entity.Property(e => e.Percentage)
                      .HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<Qualifications>().HasData(
                new Qualifications { 
                    Id = 1,
                    Course = "Computer Science", 
                    University = "Kerala University", 
                    StartYear = new DateTime(2018,1,1),
                    EndYear = new DateTime(2022,1,1),
                    Percentage = 56,
                    ApplicationUserId = "968e7c70-1abe-4d1a-a698-86550cf457f3"
                });
        }

    }
}
