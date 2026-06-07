using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Application.Concrete;
using NamSitaKaurLMS.Infrastructure.Identity;
using NamSitaKaurLMS.WebUI.Models;

namespace NamSitaKaurLMS.WebUI.Controllers
{
    [Authorize(Roles ="Student,Instructor")] // en azından giriş yapmış olsun
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserCourseService _userCourseService;
        private readonly SignInManager<AppUser> signInManager;

        public ProfileController(UserManager<AppUser> userManager, IUserCourseService userCourseService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _userCourseService = userCourseService;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId))
                return RedirectToAction("Login", "Account");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            var myCourses = _userCourseService.GetAllCoursesByUser(userId);

            var vm = new UserProfileViewModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roles
            };

            return View(vm);
        }
    
        

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (!ModelState.IsValid) return View();

            var currentUser = (await _userManager.GetUserAsync(User))!;

            var checkPassword = await _userManager.CheckPasswordAsync(currentUser, changePasswordViewModel.PasswordOld);

            if (!checkPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış" );
                return View();
            }

            var result = await _userManager.ChangePasswordAsync(currentUser!, changePasswordViewModel.PasswordOld, changePasswordViewModel.PasswordNew);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Şifre değiştirme işlemi başarısız oldu");
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(currentUser!);
            await signInManager.SignOutAsync();
            await signInManager.PasswordSignInAsync(currentUser!, changePasswordViewModel.PasswordNew, true, false);

            TempData["SuccessMessage"] = "Şifre güncelleme işlemi başarılı bir şekilde gerçekleşmiştir.";
            return View();



            return View();
        }
    
    }
}
