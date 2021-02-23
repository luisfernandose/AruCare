using AruCareApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace AruCareApi.Data
{
    public class ApplicationDbContext :
                IdentityDbContext<ApplicationUser,
                ApplicationRole, string,
                IdentityUserClaim<string>,
                ApplicationUserRole,
                IdentityUserLogin<string>,
                IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Doctor_Specialty>(dc =>
            {
                dc.HasKey(ur => new { ur.IdSpeciality, ur.IdDoctor });
            });

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }

        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<Specialty> Specialty { get; set; }
        public DbSet<Doctor_Specialty> Doctor_Specialty { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Languages> Languages { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Chat> Chat { get; set; }
    }

}
