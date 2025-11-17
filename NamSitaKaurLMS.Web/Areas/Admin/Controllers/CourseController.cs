using Microsoft.AspNetCore.Mvc;

namespace NamSitaKaurLMS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
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
