using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
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
                                                    Availability = GetAvailability(m),
                                                    WatchedDate = null,
                                                    Title = m.Name,
                                                    Cast = GetCast(m),
                                                    Summary = m.Synopsis,
                                                    RunTime = GetRunTimeInMinutes(m.Runtime),
                                                    Providers = new List<MovieProvider>{NETFLIX_DVD, NETFLIX_BLUERAY, NETFLIX_STREAM}
                                                }).ToList();
            return movies;
        }

        private IEnumerable<Title> DoSearch(NetflixCatalog context, MovieSearchCriteria criteria)
        {
            var results = new List<Title>();

            results = context.Titles.Where(t => t.Name.Equals(criteria.Title)).Select(t => t).ToList();
            if (!results.Any())
            {
                results = context.Titles.Where(t => t.Name.Contains(criteria.Title)).Select(t => t).ToList();
            }

            return results;
        }

        private IEnumerable<AvailabilityInfo> GetAvailability(Title title)
        {
            var availability = new List<AvailabilityInfo>();
            
            if(title.Instant.Available)
            {
                availability.Add(new AvailabilityInfo
                                     {
                                         AvailableFrom = title.Instant.AvailableFrom,
                                         AvailableTo = title.Instant.AvailableTo,
                                         MediaType = "Instant",
                                         ProviderName = "Netflix"
                                     });
            }

            if (title.BluRay.Available)
            {
                availability.Add(new AvailabilityInfo
                {
                    AvailableFrom = title.BluRay.AvailableFrom,
                    AvailableTo = title.BluRay.AvailableTo,
                    MediaType = "BlueRay",
                    ProviderName = "Netflix"
                });
            }

            if (title.Dvd.Available)
            {
                availability.Add(new AvailabilityInfo
                {
                    AvailableFrom = title.Dvd.AvailableFrom,
                    AvailableTo = title.Dvd.AvailableTo,
                    MediaType = "Dvd",
                    ProviderName = "Netflix"
                });
            }
            return availability;
        }

        private string GetRunTimeInMinutes(int? runtime)
        {
            if(runtime.HasValue)
            {
                var i = runtime.Value/60;
                return string.Format("{0} minutes.", i.ToString(CultureInfo.InvariantCulture));
            }
            return "0";
        }

        private List<string> GetCast(Title title)
        {
            var cast = new List<string>();
            
            if (title.Cast.Any())
            {
                cast = title.Cast.Select(c => c.Name).ToList();
            }
            return cast;

        }
    }
}