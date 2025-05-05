namespace TermProject.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
    }

}
