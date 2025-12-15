using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;

namespace NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels
{
    public class CreateLessonContentViewModel
    {

        public int CourseId { get; set; }
        public int LessonId { get; set; }
        public string LessonTitle { get; set; }
        public string ContentType { get; set; }       // Video / Audio / Text / PDF
        public string Url { get; set; }               // Video linki
        public string Text { get; set; }              // Metin açıklaması
        public int Order { get; set; }
    }
}
