using NamSitaKaurLMS.Core.Abstract;


namespace NamSitaKaurLMS.Core.Concrete
{
    public class User : EntityBase
    {


        public ICollection<CourseComment> CourseComments { get; set; }
    }
}
