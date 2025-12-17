using NamSitaKaurLMS.Core.Concrete;

namespace NamSitaKaurLMS.Application.Abstract
{
    public interface ILessonContentService
    {
        public Task AddLessonContentAsync(LessonContent lessonContent);
        public Task<IEnumerable<LessonContent>> GetLessonContentByCourseId(int courseId);
        public Task<LessonContent> GetLessonContentByLessonId(int lessonId);
        public Task DeleteCourseContentAsync(int lessonContentId, int courseId);
    }
}
