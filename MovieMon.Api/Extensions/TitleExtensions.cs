﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using MovieMon.Api.Models;
using Netflix.Catalog.v2;

namespace MovieMon.Api.Extensions
{
    public static class TitleExtensions
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
                    DeliveryFormat = "BlueRay",
                    ProviderName = "Netflix"
                });
            }

            if (title.Dvd.Available)
            {
                availability.Add(new AvailabilityInfo
                {
                    AvailableFrom = title.Dvd.AvailableFrom,
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

            if (title.Cast.Any())
            {
                cast = title.Cast.Select(c => c.Name).ToList();
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
    }
}