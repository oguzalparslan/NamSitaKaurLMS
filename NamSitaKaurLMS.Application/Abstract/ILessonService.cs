using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Application.Abstract
{
    public interface ILessonService
    {
        public Task<IEnumerable<LessonDto>> GetAllLessonsByIdAsync(int courseId);
        public Task AddLessonAsync(Lesson lesson);
    }
}
