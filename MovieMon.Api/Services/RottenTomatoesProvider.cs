using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using MovieMon.Api.Models;
using Newtonsoft.Json;
using Clips=RottenTomatoes.Types.Clips;
using Cast=RottenTomatoes.Types.Cast;
using Reviews=RottenTomatoes.Types.Reviews;
using MovieInfo=RottenTomatoes.Types.MovieInfo;
using RTMovie = RottenTomatoes.Types.Movie;
using MovieMon.Api.Extensions;


namespace MovieMon.Api.Services
{
    public class RottenTomatoesProvider:IMovieProvider
    {
        private static readonly string KEY = "xfnx2xp2tqc7mpbqmx3jet3k";
        private static readonly string MOVIE_SEARCH_URL = "http://api.rottentomatoes.com/api/public/v1.0/movies.json?apikey=xfnx2xp2tqc7mpbqmx3jet3k&q={0}";

        public RottenTomatoesProvider()
        {
            Name = "Rotten Tomatoes";
        }

        public IEnumerable<Movie> SearchMovies(MovieSearchCriteria criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria.Title)) return new List<Movie>();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
            var movieUrl = string.Format(MOVIE_SEARCH_URL, criteria.Title);
            
            client.BaseAddress = new Uri(movieUrl);
            var searchResult = client.GetStringAsync("").Result;
            var rottenTomatoesMovieList = JsonConvert.DeserializeObject<RTMovie.RootObject>(searchResult);

            var filteredList =
                rottenTomatoesMovieList.movies.Where(
                    m => m.title.Equals(criteria.Title, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!filteredList.Any())
            {
                filteredList = rottenTomatoesMovieList.movies.Where(m => m.title.ToLower().Contains(criteria.Title.ToLower())).ToList();
            }


            return (from movie in filteredList
                    let review = GetReviews(movie.id)
                    let clips = GetClips(movie.id)
                    let cast = GetCast(movie.id)
                    select new Movie
                               {
                                   Cast = cast.ToList(),
                                   RelatedClips = clips.ToList(),
                                   Reviews = review.ToList(),
                                   ProviderMovieId = movie.id,
                                   Title = movie.title,
                                   Summary = movie.synopsis,
                                   RunTime = movie.runtime,
                                   Availability = movie.GetAvailability(),
                                   RelatedImages = movie.GetRelatedImages(),
                                   MPAARating = movie.mpaa_rating,
                                   Source = "Rotten Tomatoes",
                                   Rating = movie.Ratings()
                                   
                               }).ToList();

                                      //ProviderMovieId = m.Id, 
                                      //                   Availability = m.GetAvailability(),
                                      //                   WatchedDate = null,
                                      //                   Title = m.Name,
                                      //                   Cast = m.GetCast(),
                                      //                   Summary = m.Synopsis,
                                      //                   RunTime = m.GetRunTimeInMinutes(),
                                      //                   RelatedImages = m.GetRelatedImages(),
                                      //                   MPAARating = m.Rating

//            Parallel.ForEach(filteredList, movie =>
//                                               {
//
//                                                   var review = GetReviews(movie.id);
//                                                   var clips = GetClips(movie.id);
//                                                   var cast = GetCast(movie.id);
//                                                   movieAttribute.TryAdd(movie.id,
//                                                                     new MoviePartMap
//                                                                         {
//                                                                             Title = movie.title,
//                                                                             Cast = cast.ToList(),
//                                                                             Clips = clips.ToList(),
//                                                                             Reviews = review.ToList()
//                                                                         });
//                                               });
            
           
        }

        public string Name { get; set; }

        public IEnumerable<string> GetCast(string movieId)
        {            
            var movieUrl = movieId.CastUrl();
            var cast = GetMovieAttribute<Cast.RootObject>(movieUrl);
            var casts = new List<string>();
            if (casts!=null && cast.cast!=null)
            {
                casts = cast.cast.Take(5).Select(c => c.name).ToList();
            }            
            return casts;
        }

        public IEnumerable<Review> GetReviews(string movieId)
        {            
            var reviewsUrl = movieId.ReviewsUrl();            
            var reviewsPart = GetMovieAttribute<Reviews.RootObject>(reviewsUrl);
            var reviews = new List<Review>();
            if (reviewsPart!=null && reviewsPart.reviews!=null)
            {
                reviews = reviewsPart.reviews.Select(
                    r =>
                    new Review
                        {
                            Comment = r.quote,
                            Critic = r.critic,
                            Rating = r.original_score,
                            ReviewProviderName = r.publication
                        }).ToList();

            }
            return reviews;
        }

        public IEnumerable<string> GetClips(string movieId)
        {
            var clipsUrl = movieId.ClipsUrl();
            var clipPart = GetMovieAttribute<Clips.RootObject>(clipsUrl);
            var clips = new List<string>();
            if (clipPart!=null && clipPart.clips!=null)
            {
                clips = clipPart.clips.Select(c => c.links.alternate).ToList();
            }
            return clips;
        }

        public T GetMovieAttribute<T>(string url) where T : new()
        {

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(url);
                var searchResult = client.GetStringAsync("").Result;
                T part = JsonConvert.DeserializeObject<T>(searchResult);
                return part;
            }
            catch (Exception e)
            {
                //woops nothign we can do...u just won't get this attribute
            }

            return new T();
        }

    }
}