using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Core.Dtos
{
    public class UserCoursesDto
    {
        public string Name { get; set; }
        public int CourseId { get; set; }
        public string Description { get; set; }
        public string LessonCount { get; set; }
    }
}
