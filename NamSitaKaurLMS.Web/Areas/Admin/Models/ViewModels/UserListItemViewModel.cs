namespace NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels
{
    public class UserListItemViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; }
    }
}
