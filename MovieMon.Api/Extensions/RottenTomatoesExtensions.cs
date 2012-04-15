namespace MovieMon.Api.Extensions
{
    public static class RottenTomatoesExtensions
    {
        private static readonly string MOVIE_PART_URL = "http://api.rottentomatoes.com/api/public/v1.0/movies/{0}/{1}.json?apikey=xfnx2xp2tqc7mpbqmx3jet3k";

        public static string ClipsUrl(this string movieId)
        {
            return string.Format(MOVIE_PART_URL, movieId, "clips");
        }

        public static string ReviewsUrl(this string movieId)
        {
            return string.Format(MOVIE_PART_URL, movieId, "reviews");
        }

        public static string CastUrl(this string movieId)
        {
            return string.Format(MOVIE_PART_URL, movieId, "cast");
        }
    }
}