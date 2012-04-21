﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RottenTomatoes.Types.Movie
{
    public class ReleaseDates
    {
        public string theater { get; set; }
        public string dvd { get; set; }
    }

    public class Ratings
    {
        public string critics_rating { get; set; }
        public string critics_score { get; set; }
        public string audience_rating { get; set; }
        public string audience_score { get; set; }
    }

    public class Posters
    {
        public string thumbnail { get; set; }
        public string profile { get; set; }
        public string detailed { get; set; }
        public string original { get; set; }
    }

    public class AbridgedCast
    {
        public string name { get; set; }
        public string id { get; set; }
        public List<string> characters { get; set; }
    }

    public class AlternateIds
    {
        public string imdb { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
        public string alternate { get; set; }
        public string cast { get; set; }
        public string clips { get; set; }
        public string reviews { get; set; }
        public string similar { get; set; }
    }

    public class Movy
    {
        public string id { get; set; }
        public string title { get; set; }
        public string year { get; set; }
        public string mpaa_rating { get; set; }
        public string runtime { get; set; }
        public string critics_consensus { get; set; }
        public ReleaseDates release_dates { get; set; }
        public Ratings ratings { get; set; }
        public string synopsis { get; set; }
        public Posters posters { get; set; }
        public List<AbridgedCast> abridged_cast { get; set; }
        public AlternateIds alternate_ids { get; set; }
        public Links links { get; set; }
    }

    public class Links2
    {
        public string self { get; set; }
        public string next { get; set; }
    }

    public class RootObject
    {
        public string total { get; set; }
        public List<Movy> movies { get; set; }
        public Links2 links { get; set; }
        public string link_template { get; set; }
    }
    
}