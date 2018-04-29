﻿using System;
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

        public ICollection<Movie> GetFavoriteMoviesFromDb(string userId)
        {
            ICollection<Movie> movieList = new List<Movie>();
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
                                var mov = new Movie()
                                {
                                    MovieId = reader.GetString(0)
                                };
                                movieList.Add(mov);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                // Log exception somehow
            }
            return movieList;
        }

        public bool InsertFavoriteMovieIntoDb(string userId,string movieId)
        {
            var query = "insert into dbo.UserHasMovie (UserId,MovieId) values ('" + userId + "','" + movieId + "');";

            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (con.State != System.Data.ConnectionState.Open) return false;

                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (SqlException e)
            {
                return false;
            }
        }

        public bool DeleteFavoriteMovieFromDb(string userId,string movieId)
        {
            var query = "delete from dbo.UserHasMovie where UserId = '" + userId + "' and MovieId = '" + movieId + "';";

            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    if (con.State != System.Data.ConnectionState.Open) return false;

                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    }
                    return true;
                }
            }
            catch (SqlException e)
            {
                return false;
            }
        }
    }
}
