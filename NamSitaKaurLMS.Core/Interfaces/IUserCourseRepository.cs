using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;

namespace NamSitaKaurLMS.Core.Interfaces
{
    public interface IUserCourseRepository : IRepository<UserCourse>
    {
        public Task AddRangeAsync(ICollection<UserCourse> userCourses);
        public Task<ICollection<UserCourse>> GetAllUsersByCourseAsync(int courseId);
        public Task<ICollection<CourseUserCountDto>> GetAllCourseUsers();
        public Task<ICollection<UserCourse>> GetAllCoursesByUserAsync(string userId);
    }
}
