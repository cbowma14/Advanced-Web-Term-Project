using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TermProject.Services;

namespace TermProject.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Exercise
        public async Task<IActionResult> Index(string? category, string? muscleGroup, string? equipment)
        {
            var query = _context.Exercises.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(e => e.Category == category);

            if (!string.IsNullOrEmpty(muscleGroup))
                query = query.Where(e => e.MuscleGroup == muscleGroup);

            if (!string.IsNullOrEmpty(equipment))
                query = query.Where(e => e.Equipment == equipment);

            var categories = await _context.Exercises.Select(e => e.Category).Distinct().ToListAsync();
            var muscleGroups = await _context.Exercises.Select(e => e.MuscleGroup).Distinct().ToListAsync();
            var equipments = await _context.Exercises.Select(e => e.Equipment).Distinct().ToListAsync();

            ViewBag.Categories = categories;
            ViewBag.MuscleGroups = muscleGroups;
            ViewBag.Equipments = equipments;

            var exercises = await query.ToListAsync();
            return View(exercises);
        }
    }
}
