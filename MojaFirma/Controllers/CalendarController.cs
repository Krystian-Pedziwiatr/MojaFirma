using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojaFirma.Data;
using MojaFirma.Models;
using System.Linq;

namespace MojaFirma.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalendarController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Calendar()
        {
            var vacations = await _context.Vacations
                .Include(v => v.User) 
                .ToListAsync();

            var events = vacations.Select(v => new
            {
                id = v.Id,
                title = v.VacationName,
                startDate = v.StartDate.ToString("yyyy-MM-dd"), 
                endDate = v.EndDate.AddDays(1).ToString("yyyy-MM-dd"), // Dodaj +1 dzień i formatuj datę
                userEmail = v.User.Email 
            });

            return View(events);
        }
    }
}
