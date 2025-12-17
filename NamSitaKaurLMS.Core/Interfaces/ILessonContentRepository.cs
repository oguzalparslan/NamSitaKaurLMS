using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Core.Interfaces
{
    public interface ILessonContentRepository
    {

        public Task AddCourseContent(LessonContent lessonContent);
        public Task<LessonContent> GetLessonContentByLessonId(int lessonId);
        public Task<List<LessonContent>> GetAllLessonContentsWithLessonId(int lessonId);
        public Task<List<LessonContent>> GetAllLessonContentsWithCourseId(int courseId);
        public Task<List<LessonContent>> GelAllLessonContent(int courseId);
    }
}
