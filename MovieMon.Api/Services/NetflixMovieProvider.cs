using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using MovieMon.Api.Extensions;
using MovieMon.Api.Models;
using Netflix.Catalog.v2;

namespace MovieMon.Api.Services
{
    public class NetflixMovieProvider:IMovieProvider
    {
        public NetflixMovieProvider()
        {
            Name = "Netflix";            
        }

        public IEnumerable<Movie> SearchMovies(MovieSearchCriteria criteria)
        {
            var netflixUri = new Uri("http://odata.netflix.com/Catalog/");
            var context = new NetflixCatalog(netflixUri);

            var titlesList = DoSearch(context, criteria);
            var movies = titlesList.Select(m => new Movie
                                                    {
                                                        ProviderMovieId = m.Id,
                                                        Availability = m.GetAvailability(),
                                                        WatchedDate = null,
                                                        Title = m.Name,
                                                        Cast = m.GetCast(),
                                                        Summary = m.Synopsis,
                                                        RunTime = m.GetRunTimeInMinutes(),
                                                        RelatedImages = m.GetRelatedImages(),
                                                        Reviews = m.GetReviews(),
                                                        Rating = m.GetRating(),
                                                         MPAARating = m.Rating,
                                                         Source = "Netflix"                                                                                                   
                                                     }).ToList();
            return movies;
        }

        public string Name { get; set; }

        private static IEnumerable<Title> DoSearch(NetflixCatalog context, MovieSearchCriteria criteria)
        {
            var movies = new List<Title>();
            if (!string.IsNullOrWhiteSpace(criteria.Title))
            {
                movies = context.Titles.Expand("Cast").Where(t => t.Name.ToLower().Equals(criteria.Title.ToLower())).Select(t => t).ToList();
                if (!movies.Any())
                {
                    movies =context.Titles.Expand("Cast").Where(t => t.Name.ToLower().Contains(criteria.Title.ToLower())).Select(t => t).ToList();
                }
                return movies;
            }

            if (!string.IsNullOrWhiteSpace(criteria.Genre))
            {                
                var genreList = GenreMap.GetMap(criteria.Genre);
                var genre = genreList.First();
                var byGenre = context.Genres.Expand("Titles/Cast").Where(g => g.Name.Equals(genre)).ToList();
                var titles = byGenre.SelectMany(g => g.Titles.ToList());
                movies = new List<Title>(titles);
                return movies.ToList();
            }

            return movies;
        }
    }
}