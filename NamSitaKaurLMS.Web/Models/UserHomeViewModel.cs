namespace NamSitaKaurLMS.WebUI.Models
{
    public class UserHomeViewModel
    {
        public ICollection<UserCoursesViewModel?> Courses { get; set; }
        public UserProfileViewModel userProfile { get; set; }
    }
}