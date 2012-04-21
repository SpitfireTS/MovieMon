using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using MovieMon.Api.Controllers;
using MovieMon.Api.Models;

namespace MovieMon.Api.Services
{
    public class SearchHelper
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SearchHelper));

        private IEnumerable<IMovieProvider> movieProviders = new List<IMovieProvider>()
                                                                 {
                                                                     new NetflixMovieProvider(),
                                                                     new RottenTomatoesProvider()
                                                                 };

        public IEnumerable<Movie> Search(MovieSearchCriteria criteria)
        {
            var results = new Dictionary<string, IEnumerable<Movie>>();
            foreach (var provider in movieProviders)
            {
                var movies = DoSearch(provider.SearchMovies, criteria, provider.Name);
                results.Add(provider.Name, movies);
            }
            return MergeResults(results["Netflix"], results["Rotten Tomatoes"]);
        }

        private IEnumerable<Movie> DoSearch(Func<MovieSearchCriteria, IEnumerable<Movie>> searchMovies, MovieSearchCriteria movieSearchCriteria, string providerName)
        {
            Logger.InfoFormat("Seaching {0}...", providerName);
            var results = searchMovies(movieSearchCriteria);
            var search = movieSearchCriteria.ToString();
            Logger.InfoFormat("based on the following criteria: {0} {1} returned: {2} results ", movieSearchCriteria.ToString(), providerName, results.Count());
            return results;
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
                              Key = new MovieKey { NetflixId = nf.ProviderMovieId, RottenTomatoesId = rt.ProviderMovieId },
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

            var netflixMax = netflixResults.Count();
            if (netflixMax > 12)
            {
                netflixMax = netflixMax / 2;
            }

            var rtMax = rottenTomatoesResults.Count();
            if (rtMax > 12)
            {
                rtMax = rtMax / 2;
            }

            Logger.InfoFormat("Merging additional results: added {0} from netflix and {1} from rotten tomatoes", netflixMax, rtMax);

            var merged = movies.Union(netflixResults.Take(netflixMax)).ToList();
            merged = merged.Union(rottenTomatoesResults.Take(rtMax)).ToList();

            var filtered = merged.Distinct(new MovieComparer()).ToList();
            return filtered;
        }
    }
}