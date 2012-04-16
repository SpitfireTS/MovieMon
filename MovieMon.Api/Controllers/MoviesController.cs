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
            var netFlix = new NetflixMovieProvider();
            var netflixResults = netFlix.SearchMovies(new MovieSearchCriteria { Title = name });

            var rottenTomatoes = new RottenTomatoesProvider();
            var rottenTomatoesResults = rottenTomatoes.SearchMovies(new MovieSearchCriteria {Title = name});
            var movies = MergeResults(netflixResults, rottenTomatoesResults);

            return movies;
        }

        private IEnumerable<Movie> MergeResults(IEnumerable<Movie> netflixResults, IEnumerable<Movie> rottenTomatoesResults)
        {
            var movies = new List<Movie>();
            
            movies = (from nf in netflixResults
                      from rt in rottenTomatoesResults
                      where nf.Title.ToUpper() == rt.Title.ToUpper()                      
                      select new Movie
                                 {
                                     Title = nf.Title,
                                     Availability = nf.Availability.Union(rt.Availability),
                                     Cast = rt.Cast,
                                     Key = new MovieKey{NetflixId = nf.ProviderMovieId, RottenTomatoesId = rt.ProviderMovieId, Title = nf.Title},
                                     MPAARating = rt.MPAARating,
                                     ProviderMovieId = "MovieMon",
                                     Source = "MovieMon",                                                                            
                                     RelatedClips = rt.RelatedClips,
                                     Reviews = rt.Reviews,
                                     RunTime = nf.RunTime,
                                     Summary = nf.Summary,
                                     Rating = rt.Rating,
                                     RelatedImages = nf.RelatedImages                                                                          
                                 }
                     ).ToList();
            
            if (movies.Any())
            {
                return movies;
            }

            return !rottenTomatoesResults.Any() ? netflixResults : rottenTomatoesResults;
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
