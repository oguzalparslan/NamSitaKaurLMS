using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.WebUI.Models;
using System.Security.Claims;

namespace NamSitaKaurLMS.Web.Controllers
{
    [Authorize(Roles = "Student, Instructor")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserCourseService userCourseService;

        private string CurrentUserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);


        public HomeController(ILogger<HomeController> logger, IUserCourseService userCourseService)
        {
            _logger = logger;
            this.userCourseService = userCourseService;
        }
                public async Task<IActionResult> Index()
                {
                    if (string.IsNullOrWhiteSpace(CurrentUserId))
                        return Challenge();

                    var userCourses = await userCourseService.GetAllCoursesByUser(CurrentUserId);
                    // userCourses: ICollection<UserCoursesDto>

                    var userHomeViewModel = new UserHomeViewModel
                    {
                        Courses = userCourses.Select(x => new UserCoursesViewModel
                        {
                            Name = x.Name,
                            Description = x.Description,
                            LessonCount = x.LessonCount }).OrderByDescending(x=> x.LessonCount).Take(6).ToList()
                    };

                    return View(userHomeViewModel);
                }
    }
}
