using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojaFirma.Data;

namespace MojaFirma.Controllers
{
    [Authorize(Roles = "Pracodawca")] 
    public class VacationManagerController : Controller
	{

        private readonly ApplicationDbContext _context;

        public VacationManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Akcja do wyświetlania listy urlopów
        public async Task<IActionResult> VacationManager()
        {
            var vacations = await _context.Vacations.ToListAsync();
            return View(vacations);
        }

        // Akcja do usuwania urlopu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var vacation = await _context.Vacations.FindAsync(id);
            if (vacation == null)
            {
                return NotFound();
            }

            _context.Vacations.Remove(vacation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(VacationManager));
        }
    }

    
    
}


