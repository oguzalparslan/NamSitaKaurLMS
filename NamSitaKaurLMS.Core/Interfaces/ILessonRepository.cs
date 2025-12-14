using NamSitaKaurLMS.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Core.Interfaces
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        public Task<IEnumerable<Lesson>> GetAllByCourseAsync(int courseId);
    }
}
