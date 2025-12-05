using Microsoft.EntityFrameworkCore;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Context;

namespace NamSitaKaurLMS.Infrastructure.Repository
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(NamSitaKaurLMSContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetByCategoryAsync(string category)
        {
            var coursesWithCategory = await _context.Courses
                .Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
            return coursesWithCategory;

        }

        public async Task<Course> GetCourseWithLessonsAsync(int id)
        {
            Course? course = await _context.Courses
             .Include(c => c.Lessons)
             .FirstOrDefaultAsync(c => c.Id == id);

            return course;
        }

        public async Task<IEnumerable<Course>> GetPublishedCoursesAsync()
        {
            var publishedCourses = await _context.Courses.Where(c => c.IsPublished).ToListAsync();

            return publishedCourses;

        }
    }
}
