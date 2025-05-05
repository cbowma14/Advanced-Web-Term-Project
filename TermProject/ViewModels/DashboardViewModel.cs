using TermProject.Models;

namespace TermProject.ViewModels
{
    public class DashboardViewModel
    {
        public List<Workout> RecentWorkouts { get; set; } = new();
        public List<Goal> ActiveGoals { get; set; } = new();
        public List<ProgressEntry> LatestProgressEntries { get; set; } = new();

        public int TotalWorkouts { get; set; }
        public float TotalCaloriesBurned { get; set; }  // Placeholder
    }
}