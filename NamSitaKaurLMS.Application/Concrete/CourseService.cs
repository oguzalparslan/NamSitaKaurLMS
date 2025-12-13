using NamSitaKaurLMS.Application.Abstract;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Interfaces;
using NamSitaKaurLMS.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Application.Concrete
{
    public class CourseService : ICourseService
    {

        private readonly ICourseRepository courseRepository;
        private readonly IUnitOfWork unitOfWork;

        public CourseService(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
        {
            this.courseRepository = courseRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await courseRepository.GetAllAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await courseRepository.GetByIdAsync(id);
        }

        public async Task<Course> GetWithLessonsAsync(int id)
        {
            return await courseRepository.GetCourseWithLessonsAsync(id);
        }

        public async Task CreateAsync(Course course)
        {
            await unitOfWork.Repository<Course>().AddAsync(course);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            courseRepository.UpdateCourseAsync(course);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await unitOfWork.Repository<Course>().DeleteAsync(id);
            await unitOfWork.SaveAsync();
        }
        
        public async Task AddAsync(Course course)
        {
            await unitOfWork.Repository<Course>().AddAsync(course);
            await unitOfWork.SaveAsync();
        }
    }
}
