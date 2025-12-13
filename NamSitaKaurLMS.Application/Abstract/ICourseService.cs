using NamSitaKaurLMS.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Application.Abstract
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetAllAsync(Expression<Func<Course, bool>>? filter = null);
        Task<Course> GetByIdAsync(int id);
        Task<Course> GetWithLessonsAsync(int id);
        Task CreateAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
        Task AddAsync(Course course);
    }
}
