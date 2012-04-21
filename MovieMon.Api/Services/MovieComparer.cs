using System.Collections.Generic;
using MovieMon.Api.Models;

namespace MovieMon.Api.Services
{
    internal class MovieComparer : IEqualityComparer<Movie>
    {
        public bool Equals(Movie x, Movie y)
        {
            return (x.Title.ToLower() == y.Title.ToLower());
        }

        public int GetHashCode(Movie obj)
        {
            return obj.Title.ToLower().GetHashCode();
        }
    }
}