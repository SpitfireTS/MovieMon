namespace MovieMon.Api.Extensions
{
    public static class RottenTomatoesExtensions
    {
        public static string Clips(this string url, string movieId)
        {
            return string.Format(url, movieId, "clips");
        }

        public static string Reviews(this string url, string movieId)
        {
            return string.Format(url, movieId, "reviews");
        }

        public static string Cast(this string url, string movieId)
        {
            return string.Format(url, movieId, "cast");
        }
    }
}