using NamSitaKaurLMS.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Core.Concrete
{
    public class UserLessonProgress : EntityBase
    {
        public int UserId { get; set; }
        public int LessonId { get; set; }

        public decimal WatchedPercentage { get; set; }     // %0–100
        public bool IsCompleted { get; set; }
        public DateTime LastWatchedDate { get; set; }
        public int LastWatchedSecond { get; set; }         // videonun hangi saniyesinde bırakıldı

    }
}
