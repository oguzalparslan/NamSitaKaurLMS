using NamSitaKaurLMS.Core.Abstract;
using System.ComponentModel.DataAnnotations;


namespace NamSitaKaurLMS.Core.Concrete
{
    public class CourseComment : EntityBase
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }          // 1–5 yıldız
        public string Comment { get; set; }
        public Course Course { get; set; }
        public User User { get; set; }
    }
}
