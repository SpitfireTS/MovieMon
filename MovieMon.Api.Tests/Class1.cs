using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovieMon.Api.Models;
using MovieMon.Api.Services;
using NUnit.Framework;

namespace MovieMon.Api.Tests
{
    [TestFixture]
    public class RottenTomatoesTests
    {
        [Test]
        public void GetMovie()
        {
            var provider = new RottenTomatoesProvider();
            var movies = provider.SearchMovies(new MovieSearchCriteria {Title = "God"});

        }
    }
}
