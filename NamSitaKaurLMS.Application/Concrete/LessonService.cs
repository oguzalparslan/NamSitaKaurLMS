using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Core.Dtos;
using NamSitaKaurLMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                DurationMinutes = l.DurationMinutes,
                IsPreview = l.IsPreview
            }).ToList();

            return lessonDtoList;
        }
    }
}
