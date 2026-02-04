using Microsoft.EntityFrameworkCore;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Context;

namespace NamSitaKaurLMS.Infrastructure.Repository
{
    public class UserCourseRepository : Repository<UserCourse>, IUserCourseRepository
    {
        public UserCourseRepository(NamSitaKaurLMSContext context) : base(context)
        {
        }

        public async Task AddRangeAsync(ICollection<UserCourse> userCourses)
        {
            await _context.UserCourses.AddRangeAsync(userCourses);
        }

        public async Task<ICollection<UserCourse>> GetAllUsersByCourseAsync(int courseId)
        {
            ICollection<UserCourse>? userCourses = await _context.UserCourses
                .Where(uc => uc.CourseId == courseId).AsNoTracking().ToListAsync();
            return userCourses;
        }
    }
}
