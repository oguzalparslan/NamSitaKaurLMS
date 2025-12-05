using NamSitaKaurLMS.Core.Abstract;


namespace NamSitaKaurLMS.Core.Concrete
{
    public class CourseEnvironment : EntityBase
    {
        public int CourseId { get; set; }
        public bool IsLive { get; set; }             // Canlı ders var mı?
        public bool HasDownloadableFiles { get; set; }
        public string PlatformInfo { get; set; }     // Zoom, YouTube Private, LMS Player
        public string Materials { get; set; }        // PDF, takip dosyaları

        public Course Course { get; set; }
    }
}
