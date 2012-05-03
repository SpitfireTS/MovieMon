using System;
using System.Collections.Generic;
using MovieMon.Api.Models;
using RottenTomatoes.Types.Movie;

namespace MovieMon.Api.Extensions
{
    public static class RottenTomatoesExtensions
    {
        private static readonly string MOVIE_PART_URL = "http://api.rottentomatoes.com/api/public/v1.0/movies/{0}/{1}.json?apikey=xfnx2xp2tqc7mpbqmx3jet3k";

        public static string ClipsUrl(this string movieId)
        {
            return string.Format(MOVIE_PART_URL, movieId, "clips");
        }

        public static string ReviewsUrl(this string movieId)
        {
            return string.Format(MOVIE_PART_URL, movieId, "reviews");
        }

        public static string CastUrl(this string movieId)
        {
            return string.Format(MOVIE_PART_URL, movieId, "cast");
        }

        public static Rating Ratings(this Movy movie)
        {
            var rating = movie.ratings == null
                             ? new Rating()
                             : new Rating
                                   {
                                       AudienceScore = movie.ratings.audience_score,
                                       CriticsScore = movie.ratings.critics_score
                                   };


            return rating;
        }

        public static List<RelatedImage> GetRelatedImages(this Movy title)
        {
            //need to refactor this...

            if (title.posters == null) return null;

            var relatedImages = new List<RelatedImage>
                                    {
                                        new RelatedImage{Size="Small", Url = title.posters.thumbnail},
                                        new RelatedImage{Size="Medium", Url = title.posters.profile},
                                        new RelatedImage{Size="Large", Url = title.posters.detailed},
                                        new RelatedImage{Size="HighDefinition", Url = title.posters.original}
                                    };

            return relatedImages;
        }

        public static IEnumerable<AvailabilityInfo> GetAvailability(this Movy title)
        {
            var avilability = new List<AvailabilityInfo>();
            if (title.release_dates!=null)
            {
                if (title.release_dates.theater!=null)
                {
                    DateTime date;
                    if (DateTime.TryParse(title.release_dates.theater, out date))
                    {
                        avilability.Add(new AvailabilityInfo
                                            {
                                                AvailableFrom = date,
                                                ReleaseDate = date.ToShortDateString(),
                                                DeliveryFormat = "Movie Theather", ProviderName = "Rotten Tomatoes"
                                            });
                    }
                }
            }

            if (title.release_dates != null)
            {
                if (title.release_dates.dvd != null)
                {
                    DateTime date;
                    if (DateTime.TryParse(title.release_dates.dvd, out date))
                    {
                        avilability.Add(new AvailabilityInfo { AvailableFrom = date, ReleaseDate = date.ToShortDateString(), DeliveryFormat = "Dvd", ProviderName = "Rotten Tomatoes" });
                    }
                }
            }

            return avilability;
        }
    }
}