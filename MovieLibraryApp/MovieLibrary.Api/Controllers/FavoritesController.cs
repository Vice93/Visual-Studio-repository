using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieLibrary.DbAccess;

namespace MovieLibrary.Api.Controllers
{
    public class FavoritesController : ApiController
    {
        private readonly DbConnection _dbConnection = new DbConnection();
        // GET: api/favorites
        [HttpGet]
        [Route("api/favorites")]
        public ICollection<string> Get([FromUri]string userId)
        {
            var res = _dbConnection.GetFavoriteMoviesFromDb(userId);
            return res;
        }

        // POST: api/Favorites
        [HttpPost]
        [Route("api/favorites")]
        public void Post([FromBody]string userId,string movieId)
        {
            _dbConnection.InsertFavoriteMovieIntoDb(userId,movieId);
        }

        // PUT: api/Favorites/5
        public void Put(int id, [FromBody]string value)
        {
            //Not implemented
        }

        // DELETE: api/Favorites/5
        public void Delete(string userId, string movieId)
        {

            _dbConnection.DeleteFavoriteMovieFromDb(userId,movieId);
        }
    }
}
