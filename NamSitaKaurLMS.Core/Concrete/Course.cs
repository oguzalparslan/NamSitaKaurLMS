using NamSitaKaurLMS.Core.Abstract;

namespace NamSitaKaurLMS.Core.Concrete
{
    public class Course : EntityBase
    {
        public string Title { get; set; }           // Kurs başlığı
        public string Slug { get; set; }            // URL dostu isim
        public string ThumbnailUrl { get; set; }     // Kapak görseli
        public string Level { get; set; }            // Beginner / Intermediate / Advanced
        public string Category { get; set; }         // Yoga, Meditasyon, Pranayama…
        public decimal Price { get; set; }           // Satış fiyatı
        public bool IsFree { get; set; }             // Ücretsiz kurs mı?
        public int DurationMinutes { get; set; }     // Toplam süre
        public string Language { get; set; }         // TR / EN gibi
        public bool IsPublished { get; set; }        //Kurs yayınlandı mı?

        public CourseEnvironment CourseEnvironments { get; set; }
        public ICollection<CourseComment> Comments { get; set; }
        public CourseDescription CourseDescription { get; set; }
        public ICollection<UserCourse> UserCourses { get; set; }
        public ICollection<Lesson> Lessons { get; set; }

    }
}
