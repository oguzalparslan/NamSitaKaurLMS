using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;
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

        public async Task<ICollection<CourseUserCountDto>> GetAllCourseUsers()
        {
            var courseUserCounts = await _context.UserCourses
                .IgnoreQueryFilters()
                .AsNoTracking()
                .GroupBy(uc => uc.CourseId)
                .Select(g => new CourseUserCountDto
                {
                    CourseId = g.Key,
                    UserCount = g.Count()
                })
                .OrderBy(x => x.CourseId)
                .ToListAsync();

            return courseUserCounts;
        }


        public async Task<ICollection<UserCourse>> GetAllUsersByCourseAsync(int courseId)
        {
            ICollection<UserCourse>? userCourses = await _context.UserCourses
                .Where(uc => uc.CourseId == courseId).AsNoTracking().ToListAsync();
            return userCourses;
        }

        public async Task<ICollection<UserCourse>> GetAllCoursesByUserAsync(string userId)
        {
            return await _context.UserCourses
                .Where(uc => uc.AppUserId == userId)
                .Include(uc => uc.Course)
                .ThenInclude(c => c.Lessons)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
