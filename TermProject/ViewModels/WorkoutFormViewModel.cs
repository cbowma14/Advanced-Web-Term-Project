using TermProject.Models;
using System.Collections.Generic;

namespace TermProject.ViewModels
{
    public class WorkoutFormViewModel
    {
        public Workout Workout { get; set; } = new Workout();
        public List<WorkoutExerciseInput> WorkoutExercises { get; set; } = new();
    }
}