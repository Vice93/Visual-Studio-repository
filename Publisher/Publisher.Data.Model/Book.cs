using System;

namespace Publisher.Data.Model
{
    /// <summary>
    /// Book represents the book entity
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Gets or sets the book identifier.
        /// </summary>
        /// <value>
        /// The book identifier.
        /// </value>
        public int BookId { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the date published on.
        /// </summary>
        /// <value>
        /// The date published on.
        /// </value>
        public DateTime PublishedOn { get; set; } = DateTime.Now;
    }
}
