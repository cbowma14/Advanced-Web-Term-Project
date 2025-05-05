using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TermProject.Models;
using TermProject.Services;

namespace TermProject.Controllers
{
    [Authorize]
    public class ProgressController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgressController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult MyProgress()
        {
            return View();
        }

        // GET: /Progress/Add
        public IActionResult Add()
        {
            return View(new ProgressEntry { Date = DateTime.Today });
        }

        // POST: /Progress/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProgressEntry entry)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Add", entry); // render errors in AJAX fallback
            }

            entry.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            _context.ProgressEntries.Add(entry);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}