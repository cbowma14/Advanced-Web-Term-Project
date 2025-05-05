using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TermProject.Models;
using TermProject.Services;
using TermProject.ViewModels;

namespace TermProject.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Workout/
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workouts = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .Where(w => w.UserId == userId)
                .ToListAsync();

            return View(workouts);
        }

        // GET: /Workout/Create
        public IActionResult Create()
        {
            var exercises = _context.Exercises.ToList();

            var vm = new WorkoutFormViewModel
            {
                Workout = new Workout
                {
                    Date = DateTime.Today
                },
                WorkoutExercises = exercises.Select(e => new WorkoutExerciseInput
                {
                    ExerciseId = e.Id,
                    Sets = 3,
                    Reps = 10,
                    WeightKg = 0
                }).ToList()
            };

            ViewBag.Exercises = exercises;
            return View(vm);
        }

        // POST: /Workout/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkoutFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Exercises = _context.Exercises.ToList();
                return PartialView("Create", vm);
            }
            else
            {
                vm.Workout.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                vm.Workout.WorkoutExercises = vm.WorkoutExercises
                    .Where(w => w.IsSelected)
                    .Select(w => new WorkoutExercise
                    {
                        ExerciseId = w.ExerciseId,
                        Sets = w.Sets,
                        Reps = w.Reps,
                        WeightKg = w.WeightKg
                    }).ToList();

                _context.Workouts.Add(vm.Workout);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }


        // GET: /Workout/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (workout == null) return NotFound();

            ViewBag.Exercises = _context.Exercises.ToList();
            return View(workout);
        }

        // POST: /Workout/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Workout workout, List<int> SelectedExerciseIds)
        {
            if (id != workout.Id) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                var existingWorkout = await _context.Workouts
                    .Include(w => w.WorkoutExercises)
                    .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

                if (existingWorkout == null) return NotFound();

                existingWorkout.Date = workout.Date;
                existingWorkout.Type = workout.Type;
                existingWorkout.DurationMinutes = workout.DurationMinutes;

                existingWorkout.WorkoutExercises.Clear();
                foreach (var exId in SelectedExerciseIds)
                {
                    existingWorkout.WorkoutExercises.Add(new WorkoutExercise
                    {
                        ExerciseId = exId,
                        Sets = 3,
                        Reps = 10,
                        WeightKg = 0
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Exercises = _context.Exercises.ToList();
            return View(workout);
        }

        // GET: /Workout/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (workout == null) return NotFound();

            return View(workout);
        }

        // POST: /Workout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (workout != null)
            {
                _context.WorkoutExercises.RemoveRange(workout.WorkoutExercises);
                _context.Workouts.Remove(workout);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
