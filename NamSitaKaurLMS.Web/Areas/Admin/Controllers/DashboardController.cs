using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Application.Concrete;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Identity;
using NamSitaKaurLMS.WebUI.Areas.Admin.Models;
using NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels;
using NamSitaKaurLMS.WebUI.Enums;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.WebSockets;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ICourseService courseService;
        private readonly ILessonService lessonService;
        private readonly ILessonContentService lessonContentService;
        private readonly IUserService userService;
        private readonly IUserCourseService userCourseService;

        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public DashboardController(IUnitOfWork unitOfWork,
                                   ICourseService courseService,
                                   ILessonService lessonService,
                                   ILessonContentService lessonContentService,
                                   IUserService userService,
                                   IUserCourseService userCourseService,
                                   UserManager<AppUser> userManager,
                                   RoleManager<IdentityRole> roleManager)
        {
            this.unitOfWork = unitOfWork;
            this.courseService = courseService;
            this.lessonService = lessonService;
            this.lessonContentService = lessonContentService;
            this.userService = userService;
            this.userCourseService = userCourseService;
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
            var userCourses = await userCourseService.GetAllCourseUsers(); // CourseId, UserCount

            var userCountByCourseId = userCourses.ToDictionary(x => x.CourseId, x => x.UserCount);

            var coursesViewModel = courses.Select(c =>
            {
                userCountByCourseId.TryGetValue(c.Id, out var enrolledCount);

                var remaining = c.Quota - enrolledCount;
                if (remaining < 0) remaining = 0; // istersen clamp

                return new CoursesViewModel
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
                    Environment = "Zoom",
                    RemainingQuota = remaining
                };
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

        [HttpGet]
        public async Task<IActionResult> AddUserForCoursePopup(int courseId)
        {
            var users = await userManager.Users.ToListAsync();
            var courseUsers = await userCourseService.GetUsersByCourseAsync(courseId);
            var courseUserIds = courseUsers.Select(cu => cu.AppUserId).ToHashSet();

            ICollection<AppUser> appUsers = users.Where(u => !courseUserIds.Contains(u.Id)).ToList();


            var model = new AddUserForCourseViewModel
            {
                CourseId = courseId,
                Users = appUsers
            };

            return PartialView("~/Areas/Admin/PartialViews/_AddUserForCoursePopup.cshtml", model);
        }


        [HttpGet]
        public async Task<IActionResult> ManageUserForCoursePopup(int courseId)
        {
            var users = await userManager.Users.ToListAsync();

            var courseUsers = await userCourseService.GetUsersByCourseAsync(courseId);
            var courseUserIds = courseUsers.Select(cu => cu.AppUserId).ToHashSet();

            ICollection<AppUser> appUsers = users.Where(u => courseUserIds.Contains(u.Id)).ToList();

            var model = new UserForCourseViewModel
            {
                CourseId = courseId,
                Users = appUsers
            };

            return PartialView("~/Areas/Admin/PartialViews/_ManageUserForCoursePopup.cshtml", model);
        }

        #endregion

        #region Post Actions
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseDto model)
        {
            if (!ModelState.IsValid)
                return PartialView("~/Areas/Admin/PartialViews/_CreateCoursePopup.cshtml", model);

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

            await courseService.AddAsync(course);

            return Json(new AjaxResponse(
                Success: true,
                RedirectUrl: Url.Action("Courses", "Dashboard", new { area = "Admin" })
            ));
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

        [HttpPost]
        public async Task<IActionResult> AddUsersToCourse([FromBody] AddUsersToCourseDto addUsersToCourseDto)
        {
            List<UserCourse> userCourses = new List<UserCourse>();

            try
            {
                if (addUsersToCourseDto.CourseId != 0 && addUsersToCourseDto.UserIds.Count() > 0)
                {
                    foreach (var userId in addUsersToCourseDto.UserIds)
                    {
                        AppUser? user = await userManager.FindByIdAsync(userId);

                        if (user != null)
                        {
                            UserCourse userCourse = new UserCourse
                            {
                                AppUserId = user.Id,
                                CourseId = addUsersToCourseDto.CourseId,
                                EnrollmentDate = DateTime.UtcNow,
                                ProgressPercent = 0,
                                IsCompleted = false
                            };
                            userCourses.Add(userCourse);
                        }
                    }

                    if (userCourses.Count > 0)
                    {
                        await userCourseService.AddRangeAsync(userCourses);

                        return Json(new { success = true, message = "Katılımcılar başarıyla eklendi." });
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }



            return Json(new { success = false, message = "Katılımcı ekleme işleminde hata." });
        }

        #endregion

        #region Delete Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await courseService.DeleteAsync(id);
            return RedirectToAction("Courses", "Dashboard", new { area = "Admin" });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserByCourse(int courseId, string userId)
        {
            if (courseId != 0 && !userId.IsNullOrEmpty())
            {
                try
                {
                    await userCourseService.RemoveUserByCourseAsync(courseId, userId);

                }
                catch (Exception)
                {
                    return Json(new { success = false, message = "Katılımcı silme işleminde hata." });
                }
                return Json(new { success = true, message = "Katılımcı başarıyla kurstan silindi." });
            }
            else
            {
                return Json(new { success = false, message = "Katılımcı ve kurs bulunamadı." });
            }
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLesson(CreateLessonViewModel model)
        {
            if (!ModelState.IsValid)
                return PartialView("~/Areas/Admin/PartialViews/_CreateLessonPopup.cshtml", model);

            var lesson = new Lesson()
            {
                CourseId = model.CourseId,
                Order = model.Order,
                Title = model.Title,
                LessonDate = model.LessonDate,
                DurationMinutes = model.DurationMinutes,
                IsPreview = model.IsPreview
            };

            await lessonService.AddLessonAsync(lesson);

            return Json(new
            {
                success = true,
                courseId = model.CourseId,
                redirectUrl = Url.Action("CreateCourseLesson", "Dashboard", new { area = "Admin", id = model.CourseId }),
                redirected = true
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
            if (lessonId <= 0 || courseId <= 0)
            {
                return BadRequest();
            }

            var courseContent = await lessonContentService.GetLessonContentByLessonId(lessonId);

            if (courseContent == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Silinecek içerik bulunamadı.",
                    courseId,
                    lessonId
                });
            }

            await lessonContentService.DeleteCourseContentAsync(courseContent.Id, courseId);

            return Json(new
            {
                success = true,
                message = "Silme işlemi başarılı",
                courseId,
                lessonId
            });
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
        [HttpGet]
        public IActionResult CreateUserNew()
        {
            return PartialView("~/Areas/Admin/PartialViews/_CreateUserPopupNew.cshtml");
        }

        [HttpGet]
        public IActionResult UpdateUser(string id)
        {

            var identityUser = userManager.FindByIdAsync(id).Result;
            var user = userService.GetUser(identityUser.Id).Result;
            var userViewModel = new UpdateUserViewModel()
            {
                Id = identityUser.Id,
                UserName = identityUser.UserName,
                Email = identityUser.Email,
                PhoneNumber = identityUser.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return PartialView("~/Areas/Admin/PartialViews/_UpdateUserPopup.cshtml", userViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUserRole(string userId)

        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("userId boş olamaz.");

            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }
            var userRoles = await userManager.GetRolesAsync(user);
            var currentRoleName = userRoles.FirstOrDefault();

            var roles = await roleManager.Roles.ToListAsync();

            var currentRole = !string.IsNullOrWhiteSpace(currentRoleName)
                    ? roles.FirstOrDefault(r => r.Name == currentRoleName)
                    : null;

            var userRoleViewModel = new UpdateUserRoleViewModel()
            {
                UserId = userId,
                UserName = user.UserName,
                CurrentRoleId = currentRole.Id,
                CurrentRoleName = currentRole.Name,
                NewRoleId = currentRole?.Id,
                NewRoleName = currentRole?.Name,
                AllRoles = roles
                        .OrderBy(r => r.Name)
                        .Select(r => new SelectListItem
                        {
                            Value = r.Id,
                            Text = r.Name,
                            Selected = (r.Id == currentRole?.Id)
                        })
                        .ToList()

            };


            /*
             if (string.IsNullOrWhiteSpace(userId))
                 return BadRequest("userId boş olamaz.");

                var user = await userManager.FindByIdAsync(userId);
                if (user is null)
                    return NotFound("Kullanıcı bulunamadı.");

                var userRoles = await userManager.GetRolesAsync(user);
                var currentRoleName = userRoles.FirstOrDefault(); // tek rol varsayımı

                var roles = roleManager.Roles.ToList(); // EF kullanıyorsan ToListAsync de olur

                var currentRole = !string.IsNullOrWhiteSpace(currentRoleName)
                    ? roles.FirstOrDefault(r => r.Name == currentRoleName)
                    : null;

                var vm = new UpdateUserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    CurrentRoleId = currentRole?.Id,
                    CurrentRoleName = currentRole?.Name,
                    // default seçili gelsin
                    NewRoleId = currentRole?.Id,
                    NewRoleName = currentRole?.Name,
                    AllRoles = roles
                        .OrderBy(r => r.Name)
                        .Select(r => new SelectListItem
                        {
                            Value = r.Id,
                            Text = r.Name,
                            Selected = (r.Id == currentRole?.Id)
                        })
                        .ToList()
                };
             */


            return PartialView("~/Areas/Admin/PartialViews/_UpdateUserRolePopup.cshtml", userRoleViewModel);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserNew(CreateUserViewModel model)
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

        [HttpPost]
        public IActionResult UpdateUser(UpdateUserViewModel updateUserViewModel)
        {

            if (!ModelState.IsValid)
            {
                return PartialView("~/Areas/Admin/PartialViews/_UpdateUserPopup.cshtml", updateUserViewModel);
            }

            var updatedUser = userManager.FindByIdAsync(updateUserViewModel.Id).Result;

            updatedUser.Email = updateUserViewModel.Email;
            updatedUser.PhoneNumber = updateUserViewModel.PhoneNumber;
            userManager.UpdateAsync(updatedUser).Wait();


            return RedirectToAction("GetAllUsers", "Dashboard", new { area = "Admin" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string Id)
        {

            var user = await userManager.FindByIdAsync(Id);

            if (user != null)
            {

                try
                {
                    var result = await userManager.DeleteAsync(user);
                    return RedirectToAction("GetAllUsers", "Dashboard", new { area = "Admin" });
                }
                catch (Exception e)
                {
                    throw e.InnerException;
                }
            }
            return RedirectToAction("GetAllUsers", "Dashboard", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleViewModel updateUserRoleViewModel)
        {

            var user = await userManager.FindByIdAsync(updateUserRoleViewModel.UserId);
            var currentRoles = await userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await userManager.RemoveFromRolesAsync(user, currentRoles);
            }
            var newRole = await roleManager.FindByIdAsync(updateUserRoleViewModel.NewRoleId);
            if (newRole != null)
            {
                await userManager.AddToRoleAsync(user, newRole.Name);
            }
            return RedirectToAction("GetAllUsers", "Dashboard", new { Area = "Admin" });
        }
        #endregion
        #endregion

        #region SystemSettings Operations

        #region Get Actions

        public IActionResult GetSystemSettings()
        {
            SystemSettingsViewModel systemSettingsViewModel = new SystemSettingsViewModel();
            systemSettingsViewModel.userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(systemSettingsViewModel);
        }

        #endregion

        #endregion
    }
}
