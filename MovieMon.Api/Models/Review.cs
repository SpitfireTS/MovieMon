using System;
using System.Collections.Generic;

namespace MovieMon.Api.Models
{
    public class Review
    {
        public string Rating { get; set; }
        public string Comment { get; set; }
        public string ReviewProviderName { get; set; }
        public string Critic { get; set; }
    }
}