using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Infrastructure.Repository
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(NamSitaKaurLMSContext context) : base(context)
        {
        }

        

        public Task<IEnumerable<Lesson>> GetAllByCourseAsync(int courseId)
        {
        var lessons = _context.Lessons.Where(l => l.CourseId == courseId).AsEnumerable();
            return Task.FromResult(lessons);
        }
        

    }
}
