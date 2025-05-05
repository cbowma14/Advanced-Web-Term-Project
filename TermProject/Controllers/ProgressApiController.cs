using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TermProject.Services;

namespace TermProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProgressApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProgressData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var progress = await _context.ProgressEntries
                .Where(p => p.UserId == userId)
                .OrderBy(p => p.Date)
                .Select(p => new
                {
                    date = p.Date,
                    weightKg = p.Weight,
                    bmi = p.BMI,
                    bodyFatPercentage = p.BodyFatPercentage,
                    muscleMassPercentage = p.MuscleMass
                })
                .ToListAsync();

            return Ok(progress);
        }
    }
}