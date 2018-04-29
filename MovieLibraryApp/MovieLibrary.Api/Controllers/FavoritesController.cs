using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieLibrary.DbAccess;
using MovieLibrary.Models.Model;

namespace MovieLibrary.Api.Controllers
{
    public class FavoritesController : ApiController
    {
        private readonly DbConnection _dbConnection = new DbConnection();
        // GET: api/favorites
        [HttpGet]
        [Route("api/favorites")]
        public IEnumerable<Movie> Get([FromUri]string userId)
        {
            var res = _dbConnection.GetFavoriteMoviesFromDb(userId);
            Request.CreateResponse(HttpStatusCode.OK);
            return res;
        }

        // POST: api/Favorites
        [HttpPost]
        [Route("api/favorites")]
        public HttpResponseMessage Post([FromBody]string userId,string movieId)
        {
            if(_dbConnection.InsertFavoriteMovieIntoDb(userId,movieId)) return Request.CreateResponse(HttpStatusCode.OK, "Added it");
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Something wrong happened.");

            //These could use much more work to check for everything
        }

        // PUT: api/Favorites/5
        public void Put(int id, [FromBody]string value)
        {
            //Not implemented
        }

        // DELETE: api/Favorites/5
        [HttpDelete]
        [Route("api/favorites")]
        public HttpResponseMessage Delete([FromUri]string userId, string movieId)
        {
            if(_dbConnection.DeleteFavoriteMovieFromDb(userId,movieId)) return Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Something wrong happened.");

            //These could use much more work to check for everything
        }
    }
}
