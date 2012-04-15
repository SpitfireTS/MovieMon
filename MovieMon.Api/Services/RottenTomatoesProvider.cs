using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieMon.Api.Models;

namespace MovieMon.Api.Services
{
    public class RottenTomatoesProvider:IMovieProvider
    {
        public IEnumerable<Movie> SearchMovies(MovieSearchCriteria criteria)
        {
            throw new NotImplementedException();
        }
    }
}