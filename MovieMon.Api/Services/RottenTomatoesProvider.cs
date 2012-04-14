using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using MovieMon.Api.Models;

namespace MovieMon.Api.Services
{
    public class RottenTomatoesProvider:IMovieProvider
    {
        private static readonly string KEY = "xfnx2xp2tqc7mpbqmx3jet3k";
        private static readonly string MOVIE_SEARCH_URL = "http://api.rottentomatoes.com/api/public/v1.0/movies.json?apikey=xfnx2xp2tqc7mpbqmx3jet3k&q={0}";
        private static readonly string MOVIE_PART_URL = "http://api.rottentomatoes.com/api/public/v1.0/movies/{0}/{1}.json?apikey=xfnx2xp2tqc7mpbqmx3jet3k";

        private static readonly string MOVIE_INFO_URL = "http://api.rottentomatoes.com/api/public/v1.0/movies/{0}.json?apikey=xfnx2xp2tqc7mpbqmx3jet3k";

        public IEnumerable<Movie> SearchMovies(MovieSearchCriteria criteria)
        {
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var movieUrl = string.Format(MOVIE_SEARCH_URL, criteria.Title);
            client.BaseAddress = new Uri(movieUrl);
            var resp = client.GetStringAsync("").Result;
            return null;
        }
    }
}