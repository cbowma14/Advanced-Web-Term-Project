namespace TermProject.Models
{
    public class ProgressEntry
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public DateTime Date { get; set; }
        public float Weight { get; set; }
        public float BMI { get; set; }
        public float BodyFatPercentage { get; set; }
        public float MuscleMass { get; set; }
    }
}
