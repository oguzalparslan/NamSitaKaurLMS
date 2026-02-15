using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace NamSitaKaurLMS.WebUI.ViewModels
{
    public class LessonsViewModel
    {
        public int CourseId { get; set; }
        public List<LessonItemViewModel> Lessons { get; set; } = new();
    }

    public class LessonItemViewModel
    {
        public int LessonId { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public int Order { get; set; }

        public List<CourseContentItemViewModel> Contents { get; set; } = new();
    }

    public class CourseContentItemViewModel
    {
        public int ContentId { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ContentType { get; set; }

        [StringLength(1000)]
        public string? Url { get; set; }
    }
}
