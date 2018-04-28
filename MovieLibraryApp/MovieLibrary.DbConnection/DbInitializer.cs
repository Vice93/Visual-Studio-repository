using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.DbConnection
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<LibraryContext>
    {
        /// <summary>
        /// A method that should be overridden to actually add data to the context for seeding.
        /// </summary>
        /// <param name="context">The context to seed.</param>
        protected override void Seed(LibraryContext context)
        {
            var williamShakespeare = context.Authors.Add(new Author() { FirstName = "William", LastName = "Shakespeare", DateOfBirth = new DateTime(1564, 4, 23), ImageUrl = "http://localhost:62807/api/images/WilliamShakespeare.jpg/" });
            context.Authors.Add(new Author() { FirstName = "Conn", LastName = "Iggulden", DateOfBirth = new DateTime(1971, 1, 1), ImageUrl = "http://localhost:62807/api/images/ConnIggulden.jpg/" });
            context.Authors.Add(new Author() { FirstName = "Lee", LastName = "Child", DateOfBirth = new DateTime(1954, 10, 28), ImageUrl = "http://localhost:62807/api/images/LeeChild.jpg/" });

            context.Books.Add(new Book() { Title = "Hamlet, Prince of Denmark", Authors = new List<Author>() { williamShakespeare } });
            context.Books.Add(new Book() { Title = "Othello, the Moor of Venice", Authors = new List<Author>() { williamShakespeare } });

            base.Seed(context);
        }
    }
}
