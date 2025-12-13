using NamSitaKaurLMS.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Core.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<Course> GetCourseWithLessonsAsync(int id);
        Task<IEnumerable<Course>> GetPublishedCoursesAsync();
        Task<IEnumerable<Course>> GetByCategoryAsync(string category);
        Task UpdateCourseAsync(Course course);
        Task<IEnumerable<Course>> GetAllCoursesWithExpression(Expression<Func<Course, bool>>? filter = null);
    }
}
