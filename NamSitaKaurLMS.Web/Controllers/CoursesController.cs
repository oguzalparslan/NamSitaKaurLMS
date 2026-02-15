using Microsoft.AspNetCore.Mvc;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.WebUI.Models;
using System.Security.Claims;

namespace NamSitaKaurLMS.WebUI.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IUserCourseRepository userCourseRepository;
        private readonly ICourseRepository courseRepository;

        private string CurrentUserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);


        public CoursesController(IUserCourseRepository userCourseRepository, ICourseRepository courseRepository)
        {
            this.userCourseRepository = userCourseRepository;
            this.courseRepository = courseRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userCourses = await userCourseRepository.GetAllCoursesByUserAsync(CurrentUserId);
          
                List<UserCoursesViewModel>? userCourseCourses = userCourses.Select(uc => new UserCoursesViewModel
                {
                    Name = uc.Course?.Title,
                    Description = uc.Course?.Level,
                    LessonCount = (uc.Course?.Lessons?.Count ?? 0).ToString(),
                    CourseId = uc.CourseId
                }).ToList();

            return View(userCourseCourses);
        }
    }
}
