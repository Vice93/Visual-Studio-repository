using System;
using System.Collections.Generic;

namespace MovieLibrary.Models.Model
{
    public static class User
    {
        public static Guid UserId { get; set; }
        public static ICollection<string> FavoriteMoviesIds { get; set; }
    }
}
