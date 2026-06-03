using NamSitaKaurLMS.Core.Dtos;

namespace NamSitaKaurLMS.WebUI.Areas.Admin.Models.ViewModels
{
    public class LessonViewModel
    {
        public IEnumerable<LessonDto> lessonDtoList { get; set; } = new List<LessonDto>();

        public LessonCourseHeaderViewModel Course { get; set; } = new();

        public IEnumerable<LessonContentListViewModel> lessonContentList { get; set; }
            = new List<LessonContentListViewModel>();
    }

    public class LessonCourseHeaderViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    public class LessonContentListViewModel
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string? ContentType { get; set; }
        public string? Url { get; set; }
        public string? Text { get; set; }
        public int Order { get; set; }
    }
}