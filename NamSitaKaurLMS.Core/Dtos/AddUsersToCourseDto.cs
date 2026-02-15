using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Core.Dtos
{
    public class AddUsersToCourseDto
    {
        public int CourseId { get; set; }
        public List<string> UserIds { get; set; }
    }
}
