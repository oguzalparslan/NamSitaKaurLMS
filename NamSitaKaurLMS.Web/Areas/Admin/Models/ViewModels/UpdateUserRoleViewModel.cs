using Microsoft.AspNetCore.Mvc.Rendering;

namespace NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels
{
    public class UpdateUserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CurrentRoleId { get; set; }
        public string CurrentRoleName { get; set; }
        public string NewRoleId { get; set; }
        public string NewRoleName { get; set; }
        public List<SelectListItem> AllRoles { get; set; } = new();
    }
}
