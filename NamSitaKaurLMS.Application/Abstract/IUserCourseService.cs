using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Application.Abstract
{
    public interface IUserCourseService
    {
        public Task AddRangeAsync(ICollection<UserCourse> userCourses);
        public Task<ICollection<UserCourse>> GetUsersByCourseAsync(int courseId);

        public Task RemoveUserByCourseAsync(int courseId, string userId);
    }
}
