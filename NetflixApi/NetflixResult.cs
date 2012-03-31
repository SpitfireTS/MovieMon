using System;
using System.Collections.Generic;
using WrapNetflix;

namespace NetflixApi
{
    public class NetflixResult
    {
        public string Title { get; set; }
        public int? RunningTime { get; set; }
        public string Rating { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public List<Rating> Ratings { get; set; }
        public string Quality { get; set; }
    }
}