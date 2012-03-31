using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieMon.Api.Models
{
    public class MovieSearchCriteria
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Location Location { get; set; }
    }
}

  