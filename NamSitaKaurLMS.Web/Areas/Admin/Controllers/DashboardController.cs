using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Identity;
using NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels;
using NamSitaKaurLMS.WebUI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NamSitaKaurLMS.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class DashboardController : Controller
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ICourseService courseService;
        private readonly ILessonService lessonService;
        private readonly ILessonContentService lessonContentService;
        private readonly IUserService userService;

        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public DashboardController(IUnitOfWork unitOfWork, ICourseService courseService, ILessonService lessonService, ILessonContentService lessonContentService, IUserService userService ,UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.unitOfWork = unitOfWork;
            this.courseService = courseService;
            this.lessonService = lessonService;
            this.lessonContentService = lessonContentService;
            this.userService = userService;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Course Operations

        #region Get Actions
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
        #endregion

        #region Post Actions
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
        #endregion

        #region Delete Action
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await courseService.DeleteAsync(id);
            return RedirectToAction("Courses", "Dashboard", new { area = "Admin" });
        }
        #endregion
        #endregion

        #region Lesson Operations

        #region Get Actions
        [HttpGet]
        public async Task<IActionResult> CoursesForLesson()
        {
            var courses = await courseService.GetAllAsync(x => x.Status != 2);

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
        public async Task<IActionResult> CreateCourseLesson(int id)
        {
            var course = await courseService.GetByIdAsync(id);
            if (course == null)
                return View();

            var lessonDtos = await lessonService.GetAllLessonsByIdAsync(course.Id);
            var lessonContent = await lessonContentService.GetLessonContentByCourseId(lessonDtos.Select(l => l.CourseId).FirstOrDefault());
            LessonViewModel lessonViewModel = new()
            {
                lessonDtoList = lessonDtos,
                Course = course,
                lessonContentList = lessonContent
            };
            return View(lessonViewModel);
        }
        [HttpGet]
        public IActionResult CreateLessonForm([FromQuery] int courseId)
        {
            CreateLessonViewModel createLessonViewModel = new()
            {
                CourseId = courseId
            };

            return PartialView("~/Areas/Admin/PartialViews/_CreateLessonPopup.cshtml", createLessonViewModel);
        }



        #endregion

        #region Post Actions
        [HttpPost]
        public async Task<IActionResult> CreateLesson(CreateLessonViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Areas/Admin/PartialViews/_CreateLessonPopup.cshtml", model);
            }

            var lesson = new Lesson()
            {
                CourseId = model.CourseId,
                Order = model.Order,
                Title = model.Title,
                DurationMinutes = model.DurationMinutes,
                IsPreview = model.IsPreview
            };


            await lessonService.AddLessonAsync(lesson);

            return Json(new
            {
                success = true,
                courseId = model.CourseId,
                redirectUrl = Url.Action("CreateCourseLesson", "Dashboard", new { area = "Admin", id = model.CourseId })

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLesson(int id, int courseId)
        {
            // güvenlik/validasyon
            if (id <= 0) return BadRequest();

            await lessonService.DeleteLessonAsync(id);

            return Json(new
            {
                success = true,
                courseId,
                redirectUrl = Url.Action("CreateCourseLesson", "Dashboard", new { area = "Admin", id = courseId })
            });
        }



        #endregion

        #endregion

        #region LessonContent Operations

        #region Get Actions
        [HttpGet]
        public async Task<IActionResult> CreateLessonContentForm([FromQuery] int courseId, int lessonId)
        {
            CreateLessonContentViewModel createLessonViewModel = new()
            {
                LessonId = lessonId,
                CourseId = courseId,
                LessonTitle = await lessonService.GetByLessonTitleAsync(lessonId)
            };

            return PartialView("~/Areas/Admin/PartialViews/_CreateLessonContentPopup.cshtml", createLessonViewModel);
        }
        [HttpPost]

        #endregion

        #region Post Actions
        public async Task<IActionResult> CreateLessonContent(CreateLessonContentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("~/Areas/Admin/PartialViews/_CreateLessonContentPopup.cshtml", model);
            }

            var lessonContent = new LessonContent()
            {
                ContentType = model.ContentType,
                Url = model.Url,
                Order = model.Order,
                LessonId = model.LessonId,
                Text = model.Text
            };
            await lessonContentService.AddLessonContentAsync(lessonContent);

            return Json(new
            {
                success = true,
                courseId = model.CourseId,
                redirectUrl = Url.Action("CreateCourseLesson", "Dashboard", new { area = "Admin", id = model.CourseId })

            });

        }

        #endregion

        #region Delete Actions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourseContent(int lessonId, int courseId)
        {
            await lessonContentService.DeleteCourseContentAsync(lessonId, courseId);

            return RedirectToAction("CreateCourseLesson", "Dashboard", new { area = "Admin", id = courseId });
        }
        #endregion
        #endregion

        #region User Operations

        #region Get Actions
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userManager.Users
                                         .AsNoTracking()
                                         .OrderBy(x => x.UserName)
                                         .ToListAsync();
            var userListViewModel = new List<UserListItemViewModel>(users.Count);

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                userListViewModel.Add(new UserListItemViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = roles.ToList()
                });
            }
            return View(userListViewModel);
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            return PartialView("~/Areas/Admin/PartialViews/_CreateUserPopup.cshtml");
        }
        #endregion

        #region Post Actions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return PartialView("_CreateUserModal", model);
            }

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError("", e.Description);

                Response.StatusCode = 400;
                return PartialView("_CreateUserModal", model);
            }

            const string defaultRole = "Student";

            if (!await roleManager.RoleExistsAsync(defaultRole))
            {
                var roleCreate = await roleManager.CreateAsync(new IdentityRole(defaultRole));
                if (!roleCreate.Succeeded)
                {
                    await userManager.DeleteAsync(user); // rollback
                    foreach (var e in roleCreate.Errors)
                        ModelState.AddModelError("", e.Description);

                    Response.StatusCode = 400;
                    return PartialView("_CreateUserModal", model);
                }
            }

            var addRoleResult = await userManager.AddToRoleAsync(user, defaultRole);
            if (!addRoleResult.Succeeded)
            {
                await userManager.DeleteAsync(user); // rollback
                foreach (var e in addRoleResult.Errors)
                    ModelState.AddModelError("", e.Description);

                Response.StatusCode = 400;
                return PartialView("_CreateUserModal", model);
            }
            User applicationUser = new()
            {
                AppUserId = user.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await userService.AddUserAsync(applicationUser);

            return RedirectToAction("GetAllUsers", "Dashboard", new { area = "Admin" });
        }

        #endregion
        #endregion

    }
}
