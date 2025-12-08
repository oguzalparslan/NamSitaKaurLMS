using System.ComponentModel.DataAnnotations;

namespace NamSitaKaurLMS.WebUI.Enums
{
    public enum CourseStatusEnum
    {
        [Display(Name = "Planlanan")]
        Planned = 0,
        [Display(Name = "Devam Eden")]
        Ongoing = 1,
        [Display(Name = "Tamamlandı")]
        Completed = 2
    }
}
