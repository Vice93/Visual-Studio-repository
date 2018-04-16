using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibrary.Model.Models
{
    public class Movie
    {
        public string MovieId { get; set; }
        public string MovieName { get; set; }
        public string ReleaseDate { get; set; }
        public string Description { get; set; }
        public string ImageReference { get; set; }

        //Add director and producer maybe? There is a lack of them in general so it will be null more often than not
    }
}
