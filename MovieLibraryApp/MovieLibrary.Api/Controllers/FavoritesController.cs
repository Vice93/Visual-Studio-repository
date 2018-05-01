using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieLibrary.DbAccess;
using MovieLibrary.Models.Model;
using Newtonsoft.Json;

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
            if(res != null)
            {
                Request.CreateResponse(HttpStatusCode.OK);
                return res;
            }
            Request.CreateResponse(HttpStatusCode.Conflict);
            return null;
        }

        // POST: api/Favorites
        [HttpPost]
        [Route("api/favorites")]
        public HttpResponseMessage Post()
        {
            var requestContent = Request.Content;
            var jsonContent = requestContent.ReadAsStringAsync().Result;
            var insert = JsonConvert.DeserializeObject<InsertModel>(jsonContent);

            if (_dbConnection.InsertFavoriteMovieIntoDb(insert.UserId,insert.MovieId)) return Request.CreateResponse(HttpStatusCode.OK, "Added movie");
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

    public class InsertModel
    {
        public string UserId { get; set; }
        public string MovieId { get; set; }
    }
}
