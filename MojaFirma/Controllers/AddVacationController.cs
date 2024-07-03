using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MojaFirma.Data;
using MojaFirma.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MojaFirma.Controllers
{
    [Authorize] // Zabezpiecz dostęp do tej części aplikacji
    public class AddVacationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AddVacationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AddVacation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddVacation(AddVacationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var vacation = new Vacation
                {
                    VacationName = model.VacationName,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Comment = model.Comment,
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value // Ustaw UserId na zalogowanego użytkownika
                };

                _context.Vacations.Add(vacation);
                await _context.SaveChangesAsync();

                TempData["VacationAddedMessage"] = "Poprawnie dodano urlop. Przejdź do sekcji kalendarza aby odszukać dni swojego urlopu";

                // Przekierowanie do tego samego widoku
                return RedirectToAction("AddVacation");
            }

            // Jeśli ModelState nie jest prawidłowy, zwróć formularz z błędami
            return View(model);
        }
    }
}