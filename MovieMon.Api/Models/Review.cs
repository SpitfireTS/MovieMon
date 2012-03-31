using System;
using System.Collections.Generic;

namespace MovieMon.Api.Models
{
    public class Review
    {
        public int Rating { get; set; }
        public IEnumerable<String> Comments { get; set; }
        public string ReviewProviderName { get; set; }
    }
}