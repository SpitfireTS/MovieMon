using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace MovieMon.Api.Models
{
//    Comedy, Comedies | Romantic Comedies, Family Comedies
//Action & Adventure, Action & Adventure | Adventures
//Drama, Dramas
//Romance, Romantic Comedies | Romantic Dramas | Romantic Movies
//Horror, Horror Movies | Classic Horror Movies | Sci-Fi Horror Movies
//Sci-Fi & Fantasy, Action Sci-Fi & Fantasy
    public class GenreMap
    {
        private static readonly IList<GenreMap> _genreMaps;

        static GenreMap()
        {
            _genreMaps = new List<GenreMap>(new[]
                                                {
                                                    new GenreMap
                                                        {
                                                            Name = "Comedy",
                                                            MapsTo =
                                                                new List<string>
                                                                    {"Romantic Comedies", "Family Comedies"}
                                                        },
                                                    new GenreMap
                                                        {
                                                            Name = "Action & Adventure",
                                                            MapsTo =
                                                                new List<string>
                                                                    {"Action & Adventure", "Adventures"}
                                                        },
                                                    new GenreMap
                                                        {
                                                            Name = "Romance",
                                                            MapsTo =
                                                                new List<string>
                                                                    {"Romantic Comedies", "Romantic Drama", "Romantic Movies"}
                                                        },
                                                    new GenreMap
                                                        {
                                                            Name = "Drama",
                                                            MapsTo =
                                                                new List<string>
                                                                    {"Dramas"}
                                                        },
                                                    new GenreMap
                                                        {
                                                            Name = "Horror",
                                                            MapsTo =
                                                                new List<string>
                                                                    {"Horror Movies", "Sci-Fi Horror Movies"}
                                                        },
                                                    new GenreMap
                                                        {
                                                            Name = "Sci-Fi & Fantasy",
                                                            MapsTo =
                                                                new List<string>
                                                                    {"Sci-Fi & Fantasy", "Sci-Fi Adventure"}
                                                        }
                                                });
        }

        public string Name { get; set; }
        public List<string> MapsTo { get; set; }

        public static IEnumerable<GenreMap> GetMaps()
        {
            return _genreMaps;
        }

        public static IEnumerable<string> GetMap(string genre)
        {
            return _genreMaps.Where(g => g.Name == genre).SelectMany(g => g.MapsTo);
        }
    }
}