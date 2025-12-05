using Microsoft.AspNetCore.Mvc;
using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Interfaces;

namespace NamSitaKaurLMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICourseService courseService;
        public CourseController(IUnitOfWork unitOfWork, ICourseService courseService)
        {
            this.unitOfWork = unitOfWork;
            this.courseService = courseService;
        }

        public IActionResult Index()
        {
            var courses = courseService.GetAllAsync();
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
