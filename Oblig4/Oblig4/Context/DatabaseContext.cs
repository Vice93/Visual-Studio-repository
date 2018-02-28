using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Oblig4.Model;

namespace Oblig4.Context
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Course> Courses { get; set; }

        public DatabaseContext()
        {
            Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(new DbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Student>()
                .HasMany(a => a.Courses)
                .WithMany(b => b.Students)
                .Map(m =>
                {
                    m.ToTable("StudentCourse");
                    m.MapLeftKey("StudentId");
                    m.MapRightKey("CourseId");
                });
        }
    }
}
