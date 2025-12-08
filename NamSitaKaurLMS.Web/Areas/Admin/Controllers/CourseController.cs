using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Core.Interfaces;

namespace NamSitaKaurLMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICourseService courseService;
        public AdminController(IUnitOfWork unitOfWork, ICourseService courseService)
        {
            this.unitOfWork = unitOfWork;
            this.courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await courseService.GetAllAsync();
            return View();
        }
        public async Task<IActionResult> Courses()
        {
            var courses = await courseService.GetAllAsync();
            return View();
        }

        public IActionResult DeactiveCourse()
        {
            return View();
        }
        public IActionResult CreateCourse()
        {
            return View();
        }
        public IActionResult CreateCourseContent()
        {
            return View();
        }
        public IActionResult CourseCalendar()
        {
            return View();
        }
    }
}
