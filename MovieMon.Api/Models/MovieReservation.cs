using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieMon.Api.Models
{
    public class MovieReservation
    {
        public string ProviderName { get; set; }
        public string Title { get; set; }
        public string Format { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }

}