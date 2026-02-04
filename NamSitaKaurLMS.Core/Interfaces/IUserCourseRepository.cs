using NamSitaKaurLMS.Core.Concrete;

namespace NamSitaKaurLMS.Core.Interfaces
{
    public interface IUserCourseRepository : IRepository<UserCourse>
    {
            public Task AddRangeAsync(ICollection<UserCourse> userCourses);
            public Task<ICollection<UserCourse>> GetAllUsersByCourseAsync(int courseId);
    }
}
