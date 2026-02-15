using Microsoft.AspNetCore.Mvc;
using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.WebUI.ViewModels;

namespace NamSitaKaurLMS.WebUI.Controllers
{
    public class LessonsController : Controller
    {

        private readonly ILessonService lessonService;
        private readonly ICourseService courseService;

        public LessonsController(ILessonService lessonService, ICourseService courseService)
        {
            this.lessonService = lessonService;
            this.courseService = courseService;
        }

        public async Task<IActionResult> Index(int courseId)
        {
            var lessons = await lessonService.GetAllByCourseAndAsync(courseId);
            var course = await courseService.GetByIdAsync(courseId);
            ViewData["CourseTitle"] = course.Title.ToString();
            var pageVm = new LessonsViewModel
            {
                CourseId = courseId,
                Lessons = lessons.Select(l => new LessonItemViewModel
                {
                    LessonId = l.Id,
                    Title = l.Title,
                    Order = l.Order,

                    Contents = (l.Contents).Select(cc => new CourseContentItemViewModel
                    {
                        ContentId = cc.Id,
                        Name = cc.Text,
                        ContentType = cc.ContentType,
                        Url = cc.Url
                    }).ToList()
                }).ToList()
            };

            return View(pageVm);
        }
    }
}
