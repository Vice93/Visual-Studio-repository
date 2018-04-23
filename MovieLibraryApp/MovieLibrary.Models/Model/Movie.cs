using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.Models.Model
{
    public class Movie
    {
        public string MovieId { get; set; }
        public string MovieName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public string ImageReference { get; set; }
        public string Pg { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }

        //Add director and producer maybe? There is a lack of them in general so it will be null more often than not
    }
}
