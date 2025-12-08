using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Core.Dtos
{
    public class CreateCourseDto
    {
        [Required]
        public string Title { get; set; }

        public string Slug { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Level { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public bool IsFree { get; set; }

        public int DurationMinutes { get; set; }

        public string Language { get; set; }

        public bool IsPublished { get; set; }

        public int Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Quota { get; set; }
    }

}
