using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MovieMon.Api.Models;
using MovieMon.Api.Services;

namespace MovieMon.Api.Controllers
{
    public class MoviesController : ApiController
    {
        // GET /api/moviesearch
        public IEnumerable<Movie> GetByName(string name)
        {
            var provider = new NetflixMovieProvider();
            var movies = provider.SearchMovies(new MovieSearchCriteria {Title = name});
            return movies;
        }

        public IEnumerable<Movie> GetByNameAndFormat(string name, string format)
        {
            var provider = new NetflixMovieProvider();
            var movies = provider.SearchMovies(new MovieSearchCriteria { Title = name });
            return movies;
        }
        
        // GET /api/moviesearch/5
        
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/moviesearch
        public void Post(string value)
        {
        }

        // PUT /api/moviesearch/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/moviesearch/5
        public void Delete(int id)
        {
        }
    }
}
