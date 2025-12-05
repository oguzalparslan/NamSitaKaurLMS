using NamSitaKaurLMS.Core.Abstract;

namespace NamSitaKaurLMS.Core.Concrete
{
    public class Lesson : EntityBase
    {
        public int CourseId { get; set; }
        public int Order { get; set; }                 // Sıra numarası
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsPreview { get; set; }            // Ön izleme dersi mi?

        public Course Courses { get; set; }
        public ICollection<LessonContent> Contents { get; set; }
    }
}
