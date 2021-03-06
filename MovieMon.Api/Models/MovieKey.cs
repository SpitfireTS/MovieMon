﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieMon.Api.Models
{
    public class MovieKey
    {
        public string NetflixId { get; set; }
        public string RottenTomatoesId { get; set; }       
        public bool IsInQueue { get; set; }
        public bool wasWatched { get; set;}
        public int? Rating { get; set; }
        public string Comment { get; set; }
        public string WatchedDate { get; set; }
    }
}