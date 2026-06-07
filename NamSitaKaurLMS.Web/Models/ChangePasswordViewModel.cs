using System.ComponentModel.DataAnnotations;

namespace NamSitaKaurLMS.WebUI.Models
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mevcut şifre alanı boş geçilemez.")]
        [Display(Name = "Mevcut Şifre")]
        [MinLength(6, ErrorMessage = "Mevcut şifre en az 6 karakter olmalıdır.")]
        public string PasswordOld { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni şifre alanı boş geçilemez.")]
        [Display(Name = "Yeni Şifre")]
        [MinLength(6, ErrorMessage = "Yeni şifre en az 6 karakter olmalıdır.")]
        public string PasswordNew { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare(nameof(PasswordNew), ErrorMessage = "Yeni şifre ve yeni şifre tekrarı alanları birbirini tutmalıdır.")]
        [Required(ErrorMessage = "Yeni şifre tekrarı alanı boş geçilemez.")]
        [Display(Name = "Yeni Şifre Tekrarı")]
        [MinLength(6, ErrorMessage = "Yeni şifre tekrarı en az 6 karakter olmalıdır.")]
        public string PasswordNewConfirm { get; set; } = null!;
    }
}
