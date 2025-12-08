using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NamSitaKaurLMS.Core.Concrete;
using NamSitaKaurLMS.Infrastructure.Identity;

namespace NamSitaKaurLMS.Infrastructure.Context
{
    public class NamSitaKaurLMSContext : IdentityDbContext<AppUser>
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
