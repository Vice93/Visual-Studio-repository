using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieLibrary.Models.Model;

namespace MovieLibrary.DbAccess
{
    public class DbConnection
    {
        private const string ConnectionString = @"Data Source=donau.hiof.no;Initial Catalog=jonasv;Integrated Security=False;User ID=jonasv;Password=Sp58y2;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public ICollection<string> GetFavoriteMoviesFromDb(string userId)
        {
            ICollection<string> movieList = new List<string>();
            var query = "select MovieId from dbo.UserHasMovie where UserId='" + userId + "';";
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (con.State != System.Data.ConnectionState.Open) return null;
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                movieList.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                movieList.Add("Exception: " + e.Message);
            }
            return movieList;
        }

        public void InsertFavoriteMovieIntoDb(string userId,string movieId)
        {
            var query = "insert into dbo.UserHasMovie (UserId,MovieId) values ('" + userId + "','" + movieId + "');";

            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (con.State != System.Data.ConnectionState.Open) return;

                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                        Debug.WriteLine("Inserted: " + userId + ", " + movieId);
                    }
                }
            }
            catch (SqlException e)
            {
                // Log exception somehow
            }
        }

        public void DeleteFavoriteMovieFromDb(string userId,string movieId)
        {
            var query = "delete from dbo.UserHasMovie where UserId = '" + userId + "' and MovieId = '" + movieId + "';";

            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (con.State != System.Data.ConnectionState.Open) return;

                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                //Log exception somehow
            }
        }
    }
}
