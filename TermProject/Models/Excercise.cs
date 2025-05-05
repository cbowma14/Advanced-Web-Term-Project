namespace TermProject.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string MuscleGroup { get; set; } = null!;
        public string Equipment { get; set; } = null!;

        public ICollection<WorkoutExercise>? WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}
