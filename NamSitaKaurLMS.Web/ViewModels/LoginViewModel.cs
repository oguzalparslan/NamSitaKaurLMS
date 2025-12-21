using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace NamSitaKaurLMS.WebUI.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
