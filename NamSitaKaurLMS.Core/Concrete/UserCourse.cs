using NamSitaKaurLMS.Core.Abstract;


namespace NamSitaKaurLMS.Core.Concrete
{
    public class UserCourse : EntityBase
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }

        public DateTime EnrollmentDate { get; set; }
        public DateTime? ExpirationDate { get; set; }   // abonelik süresi
        public decimal ProgressPercent { get; set; }     // % ilerleme

        public bool IsCompleted { get; set; }
        public User User { get; set; }
        public Course Course { get; set; }
    }
}
