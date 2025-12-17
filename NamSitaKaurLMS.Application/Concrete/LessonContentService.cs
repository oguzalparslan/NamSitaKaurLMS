using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Application.Concrete
{
    public class LessonContentService : ILessonContentService
    {
        private readonly ILessonContentRepository lessonContentRepository;
        private readonly IUnitOfWork unitOfWork;

        public LessonContentService(ILessonContentRepository lessonContentRepository, IUnitOfWork unitOfWork)
        {
            this.lessonContentRepository = lessonContentRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task AddLessonContentAsync(LessonContent lessonContent)
        {
            await unitOfWork.Repository<LessonContent>().AddAsync(lessonContent);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteCourseContentAsync( int lessonContentId, int courseId)
        {
            var lessonContent = await unitOfWork.Repository<LessonContent>().GetByIdAsync(lessonContentId);
            if(lessonContent == null)
            {
                throw new Exception($"Silinecek veri bulunamadı {lessonContentId}");
            }
            await unitOfWork.Repository<LessonContent>().DeleteAsync(lessonContentId);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<LessonContent>> GetLessonContentByCourseId(int courseId)
        {
            var lessonContents = await lessonContentRepository.GetAllLessonContentsWithCourseId(courseId);
            return lessonContents;
        }

        public Task<LessonContent> GetLessonContentByLessonId(int lessonId)
        {
            throw new NotImplementedException();
        }
    }
}
