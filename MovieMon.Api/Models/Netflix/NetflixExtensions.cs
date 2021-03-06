﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using MovieMon.Api.Models;
using Netflix.Catalog.v2;

namespace MovieMon.Api.Extensions
{
    public static class NetflixExtensions
    {

        public static IEnumerable<AvailabilityInfo> GetAvailability(this Title title)
        {
            var availability = new List<AvailabilityInfo>();

            if (title.Instant.Available)
            {
                availability.Add(new AvailabilityInfo
                {
                    AvailableFrom = title.Instant.AvailableFrom,
                    AvailableTo = title.Instant.AvailableTo,
                    ReleaseDate = title.Instant.AvailableFrom.HasValue ? title.Instant.AvailableFrom.Value.ToShortDateString():string.Empty,
                    DeliveryFormat = "Instant",
                    ProviderName = "Netflix"
                });
            }

            if (title.BluRay.Available)
            {
                availability.Add(new AvailabilityInfo
                {
                    AvailableFrom = title.BluRay.AvailableFrom,
                    AvailableTo = title.BluRay.AvailableTo,
                    ReleaseDate = title.Instant.AvailableFrom.HasValue ? title.Instant.AvailableFrom.Value.ToShortDateString() : string.Empty,
                    DeliveryFormat = "BlueRay",
                    ProviderName = "Netflix"
                });
            }

            if (title.Dvd.Available)
            {
                availability.Add(new AvailabilityInfo
                {
                    AvailableFrom = title.Dvd.AvailableFrom,
                    ReleaseDate = title.Instant.AvailableFrom.HasValue ? title.Instant.AvailableFrom.Value.ToShortDateString() : string.Empty,
                    AvailableTo = title.Dvd.AvailableTo,
                    DeliveryFormat = "Dvd",
                    ProviderName = "Netflix"
                });
            }
            return availability;
        }

        public static string GetRunTimeInMinutes(this Title title)
        {
            var runtime = title.Runtime;
            if (runtime.HasValue)
            {
                var i = runtime.Value / 60;
                return string.Format("{0} minutes.", i.ToString(CultureInfo.InvariantCulture));
            }
            return "0";
        }

        public static List<string> GetCast(this Title title)
        {
            var cast = new List<string>();

            var casts = title.Cast.ToList(); // catalog.Titles.Expand("Cast").Where(t => t.Name == title.Name).SelectMany(c=>c.Cast).ToList();
            if (casts.Any())
            {
                cast = title.Cast.Select(c => c.Name).ToList();
                var total = cast.Count();
                var count = total;
                if (total>=5)
                {
                    count = 5;
                }
                return cast.Take(count).ToList();
            }
            return cast;

        }

        public static List<RelatedImage> GetRelatedImages(this Title title)
        {
            //need to refactor this...

            if (title.BoxArt == null) return null;

            var relatedImages = new List<RelatedImage>
                                    {
                                        new RelatedImage{Size="Small", Url = title.BoxArt.SmallUrl},
                                        new RelatedImage{Size="Medium", Url = title.BoxArt.MediumUrl},
                                        new RelatedImage{Size="Large", Url = title.BoxArt.LargeUrl},
                                        new RelatedImage{Size="HighDefinition", Url = title.BoxArt.HighDefinitionUrl}
                                    };
            
            return relatedImages;
        }

        public static List<Review> GetReviews(this Title title)
        {
            return new List<Review>
                       {
                           new Review
                               {
                                   Comment = string.Format("{0} is a great movie!  A Must Watch!", title.Name),
                                   Critic = "DaMovieMon",
                                   Rating = title.AverageRating!=null? title.AverageRating.ToString(): "N/A",
                                   ReviewProviderName = "MovieMon"
                               }
                       };
        }

        public static Rating GetRating(this Title title)
        {
            var rating = title.AverageRating != null ? title.AverageRating.ToString() : "N/A";
            
            return new Rating {AudienceScore = rating, CriticsScore = rating};
        }
    }
}