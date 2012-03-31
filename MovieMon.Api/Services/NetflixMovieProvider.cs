using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using MovieMon.Api.GetExtensions;
using MovieMon.Api.Models;
using Netflix.Catalog.v2;

namespace MovieMon.Api.Services
{
    public class NetflixMovieProvider:IMovieProvider
    {
        private static readonly MovieProvider NETFLIX_DVD = new MovieProvider {DeliveryType = "DVD", Name = "Netflix - DVD"};
        private static readonly MovieProvider NETFLIX_STREAM = new MovieProvider { DeliveryType = "Instant", Name = "Netflix - Instant" };
        private static readonly MovieProvider NETFLIX_BLUERAY = new MovieProvider { DeliveryType = "BlueRay", Name = "Netflix - BlueRay" };
        public IEnumerable<Movie> SearchMovies(MovieSearchCriteria criteria)
        {
            var netflixUri = new Uri("http://odata.netflix.com/Catalog/");
            var context = new NetflixCatalog(netflixUri);
            var movies = new List<Movie>();

            var titlesList = DoSearch(context, criteria);
            movies = titlesList.Select(m => new Movie
                                                {
                                                    ProviderMovieId = m.Id, 
                                                    Availability = m.GetAvailability(),
                                                    WatchedDate = null,
                                                    Title = m.Name,
                                                    Cast = m.GetCast(),
                                                    Summary = m.Synopsis,
                                                    RunTime = m.GetRunTimeInMinutes(),
                                                    Providers = new List<MovieProvider> {NETFLIX_DVD, NETFLIX_BLUERAY, NETFLIX_STREAM}
                                                }).ToList();
            return movies;
        }

        private static IEnumerable<Title> DoSearch(NetflixCatalog context, MovieSearchCriteria criteria)
        {
            var results = new List<Title>();

            results = context.Titles.Where(t => t.Name.Equals(criteria.Title)).Select(t => t).ToList();
            if (!results.Any())
            {
                results = context.Titles.Where(t => t.Name.Contains(criteria.Title)).Select(t => t).ToList();
            }

            return results;
        }
    }
}