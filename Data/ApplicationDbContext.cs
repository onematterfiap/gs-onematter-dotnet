using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OneMatter.Models;

namespace OneMatter.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<Candidate> Candidates { get; set; } = null!;
        public DbSet<JobApplication> JobApplications { get; set; } = null!;


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relação 1: Candidate -> JobApplication (1-para-N)
            builder.Entity<Candidate>()
                .HasMany(c => c.Applications)
                .WithOne(ja => ja.Candidate)
                .HasForeignKey(ja => ja.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relação 2: Job -> JobApplication (1-para-N)
            builder.Entity<Job>()
                .HasMany<JobApplication>()
                .WithOne(ja => ja.Job)
                .HasForeignKey(ja => ja.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IdentityUser>(entity =>
            {
                entity.Property(e => e.EmailConfirmed).HasColumnType("NUMBER(1)");
                entity.Property(e => e.PhoneNumberConfirmed).HasColumnType("NUMBER(1)");
                entity.Property(e => e.TwoFactorEnabled).HasColumnType("NUMBER(1)");
                entity.Property(e => e.LockoutEnabled).HasColumnType("NUMBER(1)");
            });
        }
    }
}