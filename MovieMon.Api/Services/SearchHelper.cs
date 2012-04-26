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

        private static IEnumerable<Movie> DoSearch(Func<MovieSearchCriteria, IEnumerable<Movie>> searchMovies, MovieSearchCriteria movieSearchCriteria, string providerName)
        {
            Logger.InfoFormat("Seaching {0}...", providerName);
            var results = searchMovies(movieSearchCriteria);
            Logger.InfoFormat("based on the following criteria: {0} {1} returned: {2} results ", movieSearchCriteria, providerName, results.Count());
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
                              Availability = nf.Availability.Union(rt.Availability).ToList(),
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

            var netflixMax = GetMax(netflixResults.Count());
            var rtMax = GetMax(rottenTomatoesResults.Count());
            var mergedMax = GetMax(movies.Count);

            Logger.InfoFormat("Merging additional results: added {0} from netflix and {1} from rotten tomatoes", netflixMax, rtMax);
            movies = movies.Take(mergedMax).ToList();
            var merged = movies.Union(netflixResults.Take(netflixMax)).ToList();
            merged = merged.Union(rottenTomatoesResults.Take(rtMax)).ToList();

            var filtered = merged.Distinct(new MovieComparer()).ToList();
            return filtered;
        }

        private static int GetMax(int count)
        {
            return count>20 ? 20 : count;
        }
    }
}