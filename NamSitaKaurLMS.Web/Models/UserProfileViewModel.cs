using NamSitaKaurLMS.Infrastructure.Identity;

namespace NamSitaKaurLMS.WebUI.Models
{
    public class UserProfileViewModel
    {
        public AppUser appUser { get; set; }
        public string Id { get; set; } = default!;
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfileImageUrl { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}