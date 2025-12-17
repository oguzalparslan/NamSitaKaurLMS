using Microsoft.EntityFrameworkCore;
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
    public class LessonContentRepository : Repository<LessonContent>, ILessonContentRepository
    {
        public LessonContentRepository(NamSitaKaurLMSContext context) : base(context)
        {
        }

        public Task AddCourseContent(LessonContent lessonContent)
        {
            throw new NotImplementedException();
        }

        public Task<List<LessonContent>> GelAllLessonContent(int courseId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LessonContent>> GetAllLessonContentsWithCourseId(int courseId)
        {
            var lessonContents = await _context.LessonContents.Include(x=> x.Lesson).Where(lc => lc.Lesson.CourseId == courseId).ToListAsync();
            return lessonContents;
        }

        public Task<List<LessonContent>> GetAllLessonContentsWithLessonId(int lessonId)
        {
            throw new NotImplementedException();
        }

        public Task<LessonContent> GetLessonContentByLessonId(int lessonId)
        {
            throw new NotImplementedException();
        }
    }
}
