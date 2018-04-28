using System;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MovieLibrary.DbConnection
{
    public partial class LibraryContext : DbContext
    {

        public virtual DbSet<Author> Authors { get; set; }

        public virtual DbSet<Book> Books { get; set; }


        public LibraryContext()
        {
            Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(new LibraryDBInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithMany(b => b.Authors)
                .Map(m =>
                {
                    m.ToTable("AuthorBook");
                    m.MapLeftKey("AuthorId");
                    m.MapRightKey("BookId");
                });
        }
    }
}
