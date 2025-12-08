using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NamSitaKaurLMS.Infrastructure.Identity;
using NamSitaKaurLMS.WebUI.ViewModels;

namespace NamSitaKaurLMS.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(SignInManager<AppUser> signInManager,
                                 UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View(model);
            }

            // *** ÖNEMLİ ***
            // Email → Username’e çevrilmek zorunda
            var userName = user.UserName;

            var result = await _signInManager.PasswordSignInAsync(
                userName,
                model.Password,
                false,
                false
            );

            if (result.Succeeded)
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            ModelState.AddModelError("", "Email veya şifre hatalı.");
            return View(model);
        }



        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }

}
