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

        public ProfileController(UserManager<AppUser> userManager, IUserCourseService userCourseService)
        {
            _userManager = userManager;
            _userCourseService = userCourseService;
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
    }
}
