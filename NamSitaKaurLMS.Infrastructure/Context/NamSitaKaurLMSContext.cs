using Microsoft.EntityFrameworkCore;
using NamSitaKaurLMS.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamSitaKaurLMS.Infrastructure.Context
{
    public class NamSitaKaurLMSContext : DbContext
    {
        public NamSitaKaurLMSContext(DbContextOptions options) : base(options)
        {
        }

        #region DbSets
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseComment> CourseComments { get; set; }
        public DbSet<CourseEnvironment> CourseEnvironments { get; set; }
        public DbSet<CourseDescription> CourseDescriptions { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonContent> LessonContents { get; set; }
        public DbSet<SystemSetting> SystemSetting { get; set; }
public DbSet<User> Users { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<UserLessonProgress> UserLessonProgresses { get; set; }

        #endregion
    }
}
