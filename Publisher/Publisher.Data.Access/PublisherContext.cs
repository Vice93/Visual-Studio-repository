using System.Data.Entity;
using Publisher.Data.Model;

namespace Publisher.Data.Access
{
    /// <summary>
    /// Publisher context handles the database interaction
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class PublisherContext : DbContext
    {
        /// <summary>
        /// Gets or sets the books.
        /// </summary>
        /// <value>
        /// The books.
        /// </value>
        public DbSet<Book> Books { get; set; }
    }
}
