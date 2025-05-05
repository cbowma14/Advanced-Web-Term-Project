using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TermProject.Services;
using TermProject.ViewModels;

namespace TermProject.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var workouts = await _context.Workouts
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.Date)
                .Take(5)
                .ToListAsync();

            var goals = await _context.Goals
                .Where(g => g.UserId == userId && !g.IsCompleted)
                .OrderBy(g => g.Deadline)
                .ToListAsync();

            var progress = await _context.ProgressEntries
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.Date)
                .Take(3)
                .ToListAsync();

            var viewModel = new DashboardViewModel
            {
                RecentWorkouts = workouts,
                ActiveGoals = goals,
                LatestProgressEntries = progress,
                TotalWorkouts = workouts.Count,
                TotalCaloriesBurned = workouts.Count * 300  // Placeholder
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ProgressTablePartial()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var progress = await _context.ProgressEntries
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.Date)
                .Take(3)
                .ToListAsync();

            return PartialView("_ProgressTablePartial", progress);
        }

        [HttpGet]
        public async Task<IActionResult> RecentWorkoutsPartial()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workouts = await _context.Workouts
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.Date)
                .Take(5)
                .ToListAsync();

            return PartialView("_RecentWorkoutsPartial", workouts);
        }

    }
}
