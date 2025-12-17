using NamSitaKaurLMS.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Core.Dtos
{
    public class LessonDto
    {
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int Order { get; set; }
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsPreview { get; set; }
    }
}
