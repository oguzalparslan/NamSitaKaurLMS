using NamSitaKaurLMS.Core.Abstract;


namespace NamSitaKaurLMS.Core.Concrete
{
    public class LessonContent : EntityBase
    {
        public int LessonId { get; set; }
        public string ContentType { get; set; }       // Video / Audio / Text / PDF
        public string Url { get; set; }               // Video linki
        public string Text { get; set; }              // Metin açıklaması
        public int Order { get; set; }
        public Lesson Lesson { get; set; }
    }
}
