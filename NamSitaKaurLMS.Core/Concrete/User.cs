using NamSitaKaurLMS.Core.Abstract;


namespace NamSitaKaurLMS.Core.Concrete
{
    public class User : EntityBase
    {
        public string AppUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<CourseComment> CourseComments { get; set; }
    }
}
