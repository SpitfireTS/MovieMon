using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using log4net;
using MovieMon.Api.Models;
using MovieMon.Api.Services;

namespace MovieMon.Api.Controllers
{
    
    public class MoviesController : ApiController
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(MoviesController));

        // GET /api/moviesearch
        public IEnumerable<Movie> GetByName(string name)
        {
            try
            {
                Logger.InfoFormat("Received search movie request for title {0}", name);

                var netFlix = new NetflixMovieProvider();
                Logger.Info("Seaching netflix...");
                var netflixResults = netFlix.SearchMovies(new MovieSearchCriteria { Title = name });
                Logger.InfoFormat("Neflix returned: {0} results", netflixResults.Count());

                Logger.Info("Seaching Rotten Tomatoes...");
                var rottenTomatoes = new RottenTomatoesProvider();
                var rottenTomatoesResults = rottenTomatoes.SearchMovies(new MovieSearchCriteria {Title = name});
                Logger.InfoFormat("Rotten Tomatoes returned: {0} results", rottenTomatoesResults.Count());

                Logger.Info("Aggregating results...");
                var movies = MergeResults(netflixResults, rottenTomatoesResults);
                Logger.InfoFormat("Results aggregated!  {0} results were merged successfully!", movies.Count());
            
                return movies;
            }
            catch (Exception e)
            {
                Logger.Error("An error occurred while processing search request.", e);
            }
            return new List<Movie>();
        }

        private static IEnumerable<Movie> MergeResults(IEnumerable<Movie> netflixResults, IEnumerable<Movie> rottenTomatoesResults)
        {
            var movies = (from nf in netflixResults
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
                
    }
}
