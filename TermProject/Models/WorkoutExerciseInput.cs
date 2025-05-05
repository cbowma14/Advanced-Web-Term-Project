namespace TermProject.Models
{
    public class WorkoutExerciseInput
    {
        public int ExerciseId { get; set; }
        public bool IsSelected { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
        public float WeightKg { get; set; }
    }
}
