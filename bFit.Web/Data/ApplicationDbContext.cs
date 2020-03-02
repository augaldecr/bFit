using bFit.Web.Data.Entities.Profiles;
using bFit.WEB.Data.Entities;
using bFit.WEB.Data.Entities.Common;
using bFit.WEB.Data.Entities.PersonalData;
using bFit.WEB.Data.Entities.Profiles;
using bFit.WEB.Data.Entities.Workouts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bFit.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseType> ExerciseTypes { get; set; }
        public DbSet<Franchise> Franchises { get; set; }
        public DbSet<FranchiseAdmin> FranchiseAdmins { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<GymAdmin> GymAdmins { get; set; }
        public DbSet<LocalGym> Gyms { get; set; }
        public DbSet<ObesityLevel> ObesityLevels { get; set; }
        public DbSet<DataTake> PersonalData { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<SetType> SetTypes { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<SubSet> SubSets { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<WorkoutRoutine> WorkoutRoutines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => new { u.SocialSecurity, u.Email })
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
