using Microsoft.AspNetCore.Mvc;
using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Application.Concrete;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels;
using NamSitaKaurLMS.WebUI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NamSitaKaurLMS.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class DashboardController : Controller
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ICourseService courseService;

        public DashboardController(IUnitOfWork unitOfWork, ICourseService courseService)
        {
            this.unitOfWork = unitOfWork;
            this.courseService = courseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Courses()
        {
            var courses = await courseService.GetAllAsync();

            List<CoursesViewModel> coursesViewModel = courses.Select(c => new CoursesViewModel
            {
                Title = c.Title,
                Slug = c.Slug,
                ThumbnailUrl = c.ThumbnailUrl,
                Level = c.Level,
                Category = c.Category,
                Price = c.Price,
                IsFree = c.IsFree,
                DurationMinutes = c.DurationMinutes,
                Language = c.Language,
                IsPublished = c.IsPublished,
                Status = ((CourseStatusEnum)c.Status)
                                                    .GetType()
                                                    .GetField(((CourseStatusEnum)c.Status).ToString())
                                                    .GetCustomAttribute<DisplayAttribute>()?.Name,/*Enum.GetName(typeof(CourseStatusEnum), c.Status).ToString(),*/
                StartDate = c.StartDate.ToShortDateString(),
                EndDate = c.EndDate.ToShortDateString(),
                Quota = c.Quota,
                Environment = "Zoom"
            }).ToList();

            return View(coursesViewModel);
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            return PartialView("~/Areas/Admin/PartialViews/_CreateCoursePopup.cshtml");
        }
        [HttpPost]
        public IActionResult CreateCourse(CreateCourseDto model)
        {
            if (!ModelState.IsValid)
            {
                // ❗ Validation hataları varsa:
                // popup kapanmaz
                // aynı partial geri döner
                return PartialView("_CreateCoursePopup", model);
            }

            // ✅ KAYDETME İŞLEMİ
            var course = new Course
            {
                Title = model.Title,
                Category = model.Category,
                Quota = model.Quota,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Price = model.IsFree ? 0 : model.Price,
                Status = model.Status
            };

            courseService.AddAsync(course);

            // ✅ AJAX'e başarı bilgisi dön
            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("Courses", "Dashboard", new { area = "Admin" })
            });
        }


        //public IActionResult DeactiveCourse()
        //{
        //    return View();
        //}
        //public IActionResult CreateCourseContent()
        //{
        //    return View();
        //}
        //public IActionResult CourseCalendar()
        //{
        //    return View();
        //}
    }
}
