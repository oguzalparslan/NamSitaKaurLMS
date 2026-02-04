using NamSitaKaurLMS.Infrastructure.Identity;

namespace NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels
{
    public class UserForCourseViewModel
    {
        public int  CourseId { get; set; }
        public IEnumerable<AppUser> ? Users { get; set; }
    }
}
