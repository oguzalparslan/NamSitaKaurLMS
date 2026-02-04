using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;
using NamSitaKaurLMS.Core.Interfaces;

namespace NamSitaKaurLMS.Application.Concrete
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository lessonRepository;
        private readonly IUnitOfWork unitOfWork;

        public LessonService(ILessonRepository lessonRepository, IUnitOfWork unitOfWork)
        {
            this.lessonRepository = lessonRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<LessonDto>> GetAllLessonsByIdAsync(int courseId)
        {
            var lessons = await lessonRepository.GetAllByCourseAsync(courseId);
            var lessonDtoList = lessons.Select(l => new LessonDto
            {
                CourseId = l.CourseId,
                LessonId = l.Id,
                Order = l.Order,
                Title = l.Title,
                LessonDate = l.LessonDate,
                DurationMinutes = l.DurationMinutes,
                IsPreview = l.IsPreview
            }).ToList();

            return lessonDtoList;
        }
        public async Task AddLessonAsync(Lesson lesson)
        {
            await unitOfWork.Repository<Lesson>().AddAsync(lesson);
            await unitOfWork.SaveAsync();

        }
        public async Task DeleteLessonAsync(int id)
        {
            await unitOfWork.Repository<Lesson>().DeleteAsync(id);
            await unitOfWork.SaveAsync();
        }

        public async Task<string> GetByLessonTitleAsync(int id)
        {
            var lessonTitle = string.Empty;
            var lesson = await unitOfWork.Repository<Lesson>().GetByIdAsync(id);
            if (lesson != null)
                lessonTitle = lesson.Title;
            return lessonTitle;
        }
    }
}
