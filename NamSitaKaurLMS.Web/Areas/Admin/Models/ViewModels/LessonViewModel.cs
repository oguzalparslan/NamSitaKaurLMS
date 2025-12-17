using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;

namespace NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels
{
    public class LessonViewModel
    {
        //public int LessonId { get; set; }
        //public int CourseId { get; set; }
        //public string CourseTitle { get; set; }
        //public int Order { get; set; }                 // Sıra numarası
        //public string Title { get; set; }
        //public int DurationMinutes { get; set; }
        //public bool IsPreview { get; set; }            // Ön izleme dersi mi?
        public IEnumerable<LessonDto> lessonDtoList { get; set; }
        public Course Course { get; set; }
        public IEnumerable<LessonContent> lessonContentList{ get; set; }
    }
}
