using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieLibrary.DbAccess;
using MovieLibrary.Models.Model;
using Newtonsoft.Json;

namespace MovieLibrary.Api.Controllers
{
    /// <summary>
    /// The API controller for favorite movies
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class FavoritesController : ApiController
    {
        /// <summary>
        /// The database connection
        /// </summary>
        private readonly DbConnection _dbConnection = new DbConnection();

        // GET: api/favorites
        /// <summary>
        /// Gets all movies for user with userId.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/favorites")]
        public IEnumerable<Movie> Get([FromUri]string userId)
        {
            var res = _dbConnection.GetFavoriteMoviesFromDb(userId);
            
            return res;

            //These could use much more work to check for everything
        }

        // POST: api/Favorites
        /// <summary>
        /// Insert a user and its favorite movie into the database.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Update a users movie.
        /// Required if you want to store more user/movie data in the database and need the ability to change it.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        public void Put(int id, [FromBody]string value)
        {
            //Not implemented
        }

        // DELETE: api/Favorites/5
        /// <summary>
        /// Deletes the movie belonging to user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="movieId">The movie identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/favorites")]
        public HttpResponseMessage Delete([FromUri]string userId, string movieId)
        {
            if(_dbConnection.DeleteFavoriteMovieFromDb(userId,movieId)) return Request.CreateResponse(HttpStatusCode.OK);
            return Request.CreateResponse(HttpStatusCode.BadRequest, "Something wrong happened.");

            //These could use much more work to check for everything
        }
    }

    /// <summary>
    /// The model used to read the Post content.
    /// </summary>
    public class InsertModel
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the movie identifier.
        /// </summary>
        /// <value>
        /// The movie identifier.
        /// </value>
        public string MovieId { get; set; }
    }
}
