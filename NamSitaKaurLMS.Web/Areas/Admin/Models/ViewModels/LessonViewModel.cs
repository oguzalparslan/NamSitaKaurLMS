using NamSitaKaurLMS.Core.Concrete;

namespace NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels
{
    public class LessonViewModel
    {
        public int CourseId { get; set; }
        public int Order { get; set; }                 // Sıra numarası
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsPreview { get; set; }            // Ön izleme dersi mi?
    }
}
