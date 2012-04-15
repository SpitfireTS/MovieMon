using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieMon.Api.Models
{
    public class Movie
    {
        [BsonId]
        public string id { get; set; }

        //public MovieKey Key { get; set; }
        public string ProviderMovieId { get; set; }
        public string Title { get; set; }
        public DateTime? WatchedDate { get; set; }
        public IEnumerable<AvailabilityInfo> Availability { get; set; }
        public IEnumerable<MovieProvider> Providers { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public string RunTime { get; set; }
        public string MPAARating { get; set; }
        public string Summary { get; set; }
        public List<string> Cast { get; set; }
        public List<RelatedImage> RelatedImages { get; set; }
        public List<String> RelatedClips { get; set; }
    }
}