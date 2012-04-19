using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieMon.Api.Models
{
    public class MovieKey
    {
        public string NetflixId { get; set; }
        public string RottenTomatoesId { get; set; }
        public string Title { get; set; }
        public bool IsInQueue { get; set; }
        public bool markAsWatched { get; set;}
    }
}