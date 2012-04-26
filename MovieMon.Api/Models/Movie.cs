using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace MovieMon.Api.Models
{
    public class Movie
    {
        private List<AvailabilityInfo> _availabilityInfos;

        [BsonId]
        public string id { get; set; }

        public MovieKey Key { get; set; }
        public string ProviderMovieId { get; set; }
        public string Title { get; set; }
        public DateTime? WatchedDate { get; set; }
        
        public IEnumerable<AvailabilityInfo> Availability
        {
            get { return _availabilityInfos; }  
            set
            {
                if (value == null) return;
                //add redbox only if it exists in netflix on dvd

                

                _availabilityInfos = value as List<AvailabilityInfo>;
                
                if (_availabilityInfos==null) return;
                
                if (_availabilityInfos.Any(i => i.DeliveryFormat == "Dvd" && i.ProviderName=="Netflix"))
                {                
                   // if (_availabilityInfos.Any(i=>i.ProviderName=="RedBox" && i.DeliveryFormat=="Dvd")) return;
                    _availabilityInfos.Add(new AvailabilityInfo
                                               {
                                                   Addresses = GetAddressList(),
                                                   AvailableFrom = DateTime.Now,
                                                   DeliveryFormat = "Dvd",
                                                   ProviderName = "RedBox"
                                               });
                }

                if (_availabilityInfos.Any(i => i.DeliveryFormat == "BlueRay" && i.ProviderName == "Netflix"))
                {
                    //if (_availabilityInfos.Any(i => i.ProviderName == "RedBox" && i.DeliveryFormat=="BlueRay")) return;

                    _availabilityInfos.Add(new AvailabilityInfo
                                                {
                                                    Addresses = GetAddressList(),
                                                    AvailableFrom = DateTime.Now,
                                                    DeliveryFormat = "BlueRay",
                                                    ProviderName = "RedBox"
                                                });
                }
            }
        }

        public IEnumerable<MovieProvider> Providers { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public string RunTime { get; set; }
        public string MPAARating { get; set; }
        public string Summary { get; set; }
        public List<string> Cast { get; set; }
        public List<RelatedImage> RelatedImages { get; set; }
        public List<string> RelatedClips { get; set; }
        public string Source { get; set; }
        public Rating Rating { get; set; }


        public List<string> GetAddressList()
        {
            var builder = new StringBuilder();
            var addresses = new List<string>();
            builder.AppendLine("601 N 5th St");
            builder.Append("Minneapolis, MN 55403");
            addresses.Add(builder.ToString());
            builder = new StringBuilder();
            builder.AppendLine("25 University Ave SE");
            builder.AppendLine("Minneapolis, MN, 55403");
            addresses.Add(builder.ToString());
            return addresses;
        }
    }

}