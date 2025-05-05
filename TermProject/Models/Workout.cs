using System.ComponentModel.DataAnnotations;

namespace TermProject.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = null!;
        public int DurationMinutes { get; set; }

        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();

    }
}
