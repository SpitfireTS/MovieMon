using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Contrib;
using Netflix.Catalog.v2;
using WrapNetflix;

namespace NetflixApi
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CallNFAgain();
            //CallNextFlixRestFul();
            //    Console.WriteLine("Netflix Movies Available in Streaming");
            //    CallNextFlix();
            //    Console.WriteLine("*********************************************************************");
            //    Console.WriteLine("Rotten Tomatoes Response");
            //    CallRottenTomatoes();
            //    Console.ReadLine();

            }

        //private static void CallNextFlixRestFul()
        //{
        //    var api = "http://api.netflix.com/catalog/titles";
        //    var consumerKey = "bnyqnauey6e94xbdpws6f9sk";
        //    var consumerSecret = "vKzgfFgdvP";
        //}

        private static void CallNextFlix()
            {
                var netflixUri = new Uri("http://odata.netflix.com/Catalog/");
                var context = new NetflixCatalog(netflixUri);

                var titlesList =
                    context.Titles.Where(t => t.Instant.Available).Select(t => t).ToList();

                var filteredTitles = titlesList.Select(
                    t =>
                    new NetflixResult
                        {Title = t.Name, 
                            RunningTime = t.Runtime, 
                            Rating = t.Rating, 
                            To = t.Instant.AvailableTo, 
                            From = t.Instant.AvailableFrom,
                            Quality = t.ScreenFormats.First().DeliveryFormat
                        }).
                    ToList();


                filteredTitles.ForEach(
                    (t) =>
                    Console.WriteLine(GetFormatedLine(t)));

                Console.WriteLine("Total number of titles available for streaming: " + filteredTitles.Count);

            }

            private static string GetFormatedLine(NetflixResult movie)
            {
                var builder = new StringBuilder();
                builder.AppendLine("Movie Title: " + movie.Title);
                builder.AppendLine("Rating: " + movie.Rating);
                builder.AppendLine("Running Time: " + (movie.RunningTime/60) + " minutes");
                builder.AppendLine("Available From: " + movie.From);
                builder.AppendLine("Available To: " + movie.To);
                
                return builder.ToString();
            }

            private static void CallRottenTomatoes(string name=null)
            {
                var url = @"http://api.rottentomatoes.com/api/public/v1.0.json?apikey=xfnx2xp2tqc7mpbqmx3jet3k";
                var client = new RestClient();
                var request = new RestRequest(url);
                var response = client.Execute(request);

                var movies =
                    @"http://api.rottentomatoes.com/api/public/v1.0/movies.json?apikey=xfnx2xp2tqc7mpbqmx3jet3k";
                request = new RestRequest(url);


                response = client.Execute(request);

                var reviews =
                    @"http://api.rottentomatoes.com/api/public/v1.0/movies/770672122/reviews.json?apikey=xfnx2xp2tqc7mpbqmx3jet3k";

                response = client.Execute(request);
                Console.WriteLine(response.Content);

            }


            private static void CallNextFlixRestFul()
            {
                var baseUrl = "http://api.netflix.com/";
                var client = new RestClient(baseUrl);
                var consumerKey = "bnyqnauey6e94xbdpws6f9sk";
                var consumerSecret = "vKzgfFgdvP";

                client.Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);
                var req = new RestRequest("catalog/titles");
                var resp = client.Execute(req);

           }

            private static string GetUrl(string consumerKey, NameValueCollection qs)
            {
                qs.Remove("login_url");
                qs.Remove("oauth_token_secret");
                qs["oauth_consumer_key"] = consumerKey;

                var url = "https://api-user.netflix.com/oauth/login?" + qs;
                return url;
            }
            
            public static void CallNFAgain()
            {
                var baseUrl = "http://api.netflix.com/";
                var consumerKey = "bnyqnauey6e94xbdpws6f9sk";
                var consumerSecret = "vKzgfFgdvP";
                
                var connection = new NetflixConnection(consumerKey, consumerSecret);
                var requestToken = connection.GenerateRequestToken();

                var url = requestToken.PermissionUrl;
                Process.Start(requestToken.PermissionUrl);
                var accessToken = requestToken.ConvertToAccessToken();


            }
    }

 
}
//var request = new RestRequest("oauth/request_token");
//var response = client.Execute(request);

//var qs = HttpUtility.ParseQueryString(response.Content);
//var oauth_token = qs["oauth_token"];
//var oauth_token_secret = qs["oauth_token_secret"];

//// BREAKPOINT HERE
//// Take this URL and put it in your browser
//var url = GetUrl(consumerKey, qs);

//request = new RestRequest("oauth/access_token");
//client.Authenticator = OAuth1Authenticator.ForAccessToken(
//    consumerKey, consumerSecret, oauth_token, oauth_token_secret
//    );
//response = client.Execute(request);

//request = new RestRequest("catalog/titles");
//response = client.Execute(request);
