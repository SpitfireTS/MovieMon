﻿using System;
using System.Collections.Generic;
using MovieMon.Api.Models;

namespace MovieMon.Api.Data
{
    public class MovieKeyComparer : IEqualityComparer<Movie>
    {
        public bool Equals(Movie x, Movie y)
        {
            if(x.Key!=null && y.Key!=null)
            {
                return x.Key.NetflixId == y.Key.NetflixId;
            }
            return (x.Key==y.Key);
        }

        public int GetHashCode(Movie obj)
        {
            var key = obj.Key;
            if (key!=null)
            {
                return key.NetflixId != null ? key.NetflixId.GetHashCode() : key.RottenTomatoesId.GetHashCode();
            }

            return 0;
        }
    }
}