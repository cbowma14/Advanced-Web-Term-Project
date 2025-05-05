using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TermProject.Models;

namespace TermProject.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<ProgressEntry> ProgressEntries { get; set; }
        public DbSet<Goal> Goals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId);

            builder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(we => we.ExerciseId);

            // Seed data for Exercises
            builder.Entity<Exercise>().HasData(
                new Exercise { Id = 1, Name = "Bench Press", Category = "Strength", MuscleGroup = "Chest", Equipment = "Barbell" },
                new Exercise { Id = 2, Name = "Incline Dumbbell Press", Category = "Strength", MuscleGroup = "Chest", Equipment = "Dumbbell" },
                new Exercise { Id = 3, Name = "Push-Up", Category = "Bodyweight", MuscleGroup = "Chest", Equipment = "None" },
                new Exercise { Id = 4, Name = "Pull-Up", Category = "Bodyweight", MuscleGroup = "Back", Equipment = "Pull-Up Bar" },
                new Exercise { Id = 5, Name = "Deadlift", Category = "Strength", MuscleGroup = "Back", Equipment = "Barbell" },
                new Exercise { Id = 6, Name = "Bent Over Row", Category = "Strength", MuscleGroup = "Back", Equipment = "Barbell" },
                new Exercise { Id = 7, Name = "Squat", Category = "Strength", MuscleGroup = "Legs", Equipment = "Barbell" },
                new Exercise { Id = 8, Name = "Lunges", Category = "Strength", MuscleGroup = "Legs", Equipment = "Bodyweight" },
                new Exercise { Id = 9, Name = "Leg Press", Category = "Strength", MuscleGroup = "Legs", Equipment = "Machine" },
                new Exercise { Id = 10, Name = "Overhead Press", Category = "Strength", MuscleGroup = "Shoulders", Equipment = "Dumbbell" },
                new Exercise { Id = 11, Name = "Shoulder Press Machine", Category = "Strength", MuscleGroup = "Shoulders", Equipment = "Machine" },
                new Exercise { Id = 12, Name = "Plank", Category = "Core", MuscleGroup = "Abs", Equipment = "None" },
                new Exercise { Id = 13, Name = "Crunches", Category = "Core", MuscleGroup = "Abs", Equipment = "None" },
                new Exercise { Id = 14, Name = "Russian Twists", Category = "Core", MuscleGroup = "Abs", Equipment = "None" },
                new Exercise { Id = 15, Name = "Bicep Curl", Category = "Strength", MuscleGroup = "Arms", Equipment = "Dumbbell" },
                new Exercise { Id = 16, Name = "Tricep Pushdown", Category = "Strength", MuscleGroup = "Arms", Equipment = "Cable Machine" }
            );
        }
    }
}