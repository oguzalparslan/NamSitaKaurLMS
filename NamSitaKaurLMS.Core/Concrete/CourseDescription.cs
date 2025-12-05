using NamSitaKaurLMS.Core.Abstract;

namespace NamSitaKaurLMS.Core.Concrete
{
    public class CourseDescription : EntityBase
    {
        public int CourseId { get; set; }
        public string ShortDescription { get; set; }     // Kısa açıklama
        public string LongDescription { get; set; }      // Detaylı içerik (HTML destekli)
        public string Requirements { get; set; }         // Mat, blok, kayış gibi ihtiyaçlar
        public string TargetAudience { get; set; }       // Kimler için uygun
        public string Benefits { get; set; }             // Kazanımlar
        public Course Course { get; set; }
    }
}
