using Microsoft.IdentityModel.Tokens;
using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Application.Concrete
{
    public class UserCourseService : IUserCourseService
    {
        public IUserCourseRepository userCourseRepository { get; set; }
        public IUnitOfWork unitOfWork { get; set; }

        public UserCourseService(IUserCourseRepository userCourseRepository, IUnitOfWork unitOfWork)
        {
            this.userCourseRepository = userCourseRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task AddRangeAsync(ICollection<UserCourse> userCourses)
        {
            await userCourseRepository.AddRangeAsync(userCourses);
            await unitOfWork.SaveAsync();
        }

        public async Task<ICollection<UserCourse>> GetUsersByCourseAsync(int courseId)
        {
            ICollection<UserCourse> courseUsers = await userCourseRepository.GetAllUsersByCourseAsync(courseId);

            return courseUsers;
        }

        public async Task RemoveUserByCourseAsync(int courseId, string userId)
        {
            var courseUsers = await userCourseRepository.GetAllUsersByCourseAsync(courseId);
            var courseUser = courseUsers.Where(u => u.AppUserId == userId).FirstOrDefault();

            if (courseUser != null)
            {
                try
                {
                    await userCourseRepository.DeleteAsync(courseUser.Id);
                    await unitOfWork.SaveAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<ICollection<CourseUserCountDto>> GetAllCourseUsers()
        {

            var courseUsersCount = await userCourseRepository.GetAllCourseUsers();
            return courseUsersCount;
        }

        public async Task<ICollection<UserCoursesDto>> GetAllCoursesByUser(string userId)
        {
            var userCourses = await userCourseRepository.GetAllCoursesByUserAsync(userId);

            
            var coursesByUser = userCourses
                .Where(x => x.Course.Status == 1)
                .Select(x => new UserCoursesDto
                {
                    Name = x.Course?.Title,
                    CourseId = x.CourseId,
                    Description = x.Course?.Level,
                    LessonCount = (x.Course?.Lessons?.Count ?? 0).ToString()
                })
                .ToList();

            return coursesByUser;
        }


    }
}
