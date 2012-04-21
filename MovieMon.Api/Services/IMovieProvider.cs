using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieMon.Api.Models;

namespace MovieMon.Api.Services
{
    public interface IMovieProvider
    {
        IEnumerable<Movie> SearchMovies(MovieSearchCriteria criteria);
        string Name { get; set; }
    }
}