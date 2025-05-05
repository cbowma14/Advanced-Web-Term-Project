using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TermProject.Models;
using TermProject.Services;

namespace TermProject.Controllers
{
    [Authorize]
    public class GoalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GoalController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var goals = _context.Goals
                .Where(g => g.UserId == userId)
                .OrderBy(g => g.Deadline)
                .ToList();

            return View(goals);
        }

        public IActionResult Create()
        {
            return View(new Goal { Deadline = DateTime.Today.AddDays(30) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Goal goal)
        {
            if (!ModelState.IsValid)
                return View(goal);

            goal.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var goal = await _context.Goals.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

            if (goal == null) return NotFound();

            return View(goal);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Goal updatedGoal)
        {
            if (id != updatedGoal.Id) return NotFound();

            if (!ModelState.IsValid) return View(updatedGoal);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingGoal = await _context.Goals.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);
            if (existingGoal == null) return NotFound();

            existingGoal.Description = updatedGoal.Description;
            existingGoal.Deadline = updatedGoal.Deadline;
            existingGoal.IsCompleted = updatedGoal.IsCompleted;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var goal = await _context.Goals.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

            if (goal == null) return NotFound();

            return View(goal);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var goal = await _context.Goals.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

            if (goal != null)
            {
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
