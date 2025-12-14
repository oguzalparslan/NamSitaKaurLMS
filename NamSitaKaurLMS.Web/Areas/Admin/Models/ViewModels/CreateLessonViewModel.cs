using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Core.Dtos;

namespace NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels
{
    public class CreateLessonViewModel
    {
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        [ValidateNever]
        public string CourseTitle { get; set; }
        public int Order { get; set; }                 // Sıra numarası
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsPreview { get; set; }            // Ön izleme dersi mi?

    }
}
