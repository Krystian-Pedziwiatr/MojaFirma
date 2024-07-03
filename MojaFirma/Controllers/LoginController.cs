using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MojaFirma.Data;
using MojaFirma.Models;
using System.Threading.Tasks;

namespace MojaFirma.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            var roles = _roleManager.Roles.ToList(); // Pobierz wszystkie role z bazy danych
            ViewBag.Roles = new SelectList(roles, "Name", "Name"); // Przekazuje listę ról do widoku
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Pobierz wybraną rolę z formularza
                    var selectedRole = model.Role;

                    // Sprawdź, czy rola już istnieje
                    if (!await _roleManager.RoleExistsAsync(selectedRole))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(selectedRole));
                    }

                    // Przypisz rolę do użytkownika
                    await _userManager.AddToRoleAsync(user, selectedRole);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Jeśli doszło do tego miejsca, coś poszło nie tak, zwróć formularz z błędami
            var roles = _roleManager.Roles.ToList(); // Pobierz wszystkie role
            ViewBag.Roles = new SelectList(roles, "Name", "Name"); // Przekazuje listę ról do widoku
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        if (roles.Contains("Pracownik"))
                        {
                            return RedirectToAction("PracownikAction", "PracownikController");
                        }
                        else if (roles.Contains("Pracodawca"))
                        {
                            return RedirectToAction("PracodawcaAction", "PracodawcaController");
                        }
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Nieprawidłowa próba logowania.");
            }

            return View(model);
        }
    }
}
