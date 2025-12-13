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
                Id = c.Id,
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
                                                    .GetCustomAttribute<DisplayAttribute>()?.Name,
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
        public IActionResult CreateCourse(CourseDto model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Areas/Admin/PartialViews/_CreateCoursePopup.cshtml", model);
            }

            // ✅ KAYDETME İŞLEMİ
            var course = new Course
            {
                Title = model.Title,
                Slug = model.Slug,
                ThumbnailUrl = model.ThumbnailUrl,
                Level = model.Level,
                Category = model.Category,
                Price = model.Price,
                IsFree = model.IsFree,
                DurationMinutes = model.DurationMinutes,
                Language = model.Language,
                IsPublished = model.IsPublished,
                Status = model.Status,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Quota = model.Quota,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true
            };


            courseService.AddAsync(course);

            // ✅ AJAX'e başarı bilgisi dön
            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("Courses", "Dashboard", new { area = "Admin" })
            });
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCourse([FromQuery] int id)
        {
            var course = await courseService.GetByIdAsync(id);
            if (course == null) return NotFound();

            var vm = new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Slug = course.Slug,
                ThumbnailUrl = course.ThumbnailUrl,
                Level = course.Level,
                Category = course.Category,
                Price = course.Price,
                IsFree = course.IsFree,
                DurationMinutes = course.DurationMinutes,
                Language = course.Language,
                IsPublished = course.IsPublished,
                Status = course.Status,
                Quota = course.Quota,
                StartDate = course.StartDate,
                EndDate = course.EndDate
            };
            return PartialView("~/Areas/Admin/PartialViews/_UpdateCoursePopup.cshtml", vm);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCourse(CourseDto model)
        {
            if (!ModelState.IsValid)
                return PartialView("~/Areas/Admin/PartialViews/_UpdateCoursePopup.cshtml", model);

            var course = new Course
            {
                Id = model.Id.Value,
                Title = model.Title,
                Slug = model.Slug,
                ThumbnailUrl = model.ThumbnailUrl,
                Level = model.Level,
                Category = model.Category,
                Price = model.Price,
                IsFree = model.IsFree,
                DurationMinutes = model.DurationMinutes,
                Language = model.Language,
                IsPublished = model.IsPublished,
                Status = model.Status,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Quota = model.Quota,
                UpdatedDate = DateTime.UtcNow
            };

            await courseService.UpdateAsync(course);

            return Json(new { success = true });
        }

        public async Task<IActionResult> DeleteCourse(int id)
        {
            await courseService.DeleteAsync(id);
            return RedirectToAction("Courses", "Dashboard", new { area = "Admin" });
        }
    }
}
