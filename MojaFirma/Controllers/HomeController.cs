using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojaFirma.Data;
using System.Security.Claims;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Privacy()
    {
        return View("Privacy");
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            // Pobierz ID zalogowanego użytkownika
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                // Pobierz wszystkie urlopy użytkownika
                var vacations = await _context.Vacations
                    .Where(v => v.UserId == userId)
                    .ToListAsync();

                // Oblicz wykorzystane dni urlopu
                int usedVacationDays = 0;
                foreach (var vacation in vacations)
                {
                    usedVacationDays += (int)(vacation.EndDate - vacation.StartDate).TotalDays + 1;
                }

                // Liczba dni urlopu przyznanych każdemu pracownikowi
                int totalVacationDays = 26;

                // Oblicz dostępne dni urlopu
                int availableVacationDays = totalVacationDays - usedVacationDays;

                ViewData["UsedVacationDays"] = usedVacationDays;
                ViewData["AvailableVacationDays"] = availableVacationDays;
                
            }
            return View("Home");
        }

        else
        {

            return View();
        }


        }
   }
    
