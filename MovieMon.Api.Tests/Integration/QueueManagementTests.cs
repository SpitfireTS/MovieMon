//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using log4net;
//using MovieMon.Api.Controllers;
//using MovieMon.Api.Data;
//using MovieMon.Api.Models;
//using MovieMon.Api.Services;
//using Newtonsoft.Json;
//using NUnit.Framework;

//namespace MovieMon.Api.Tests.Integration
//{
//    [TestFixture]
//    public class QueueManagementTests
//    {
//        private string movieMonRoot = null;

//        [TestFixtureSetUp]
//        public void init()
//        {
//            //set the environment to the right root url.  this needs to be like this
//            //due to App Harbor's conventions. see: http://support.appharbor.com/kb/getting-started/managing-environments

//            var env = ConfigurationManager.AppSettings["Environment"];
//            if (env=="Test")
//            {
//                movieMonRoot = "http://movieman.apphb.com/api/";
                
//            }else if (env=="Debug")
//            {
//                movieMonRoot = "http://ipv4.fiddler/MovieMon/api/";
//            } 

//            //otherwise let the test fail...those are the only envs we should be running tests on...
//        }

//        [Test]
//        public void GetMembers_WhenInvoked_RetrievesDefaultMemember()
//        {
//            //arrange
//            var membersUrl = MakeUrl("Members");
//            var client = new HttpClient();
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//            client.BaseAddress = new Uri(membersUrl);

//            //act
//            var result = client.GetStringAsync("").Result;

//            //assert
//            var members = JsonConvert.DeserializeObject<IEnumerable<Member>>(result);
//            var mmMember = members.FirstOrDefault(m => m.Name == "MovieMonFan");
//            Assert.That(mmMember != null);
//        }

//        [Test]
//        public void GetMember_WhenInvoked_DefaultMovieIsFetchedFromProviders()
//        {
//            //arrange
//            var membersUrl = MakeUrl("Members/f98b9048-1324-440f-802f-ebcfab1c5395");
//            var client = new HttpClient();
//            //get the member
//            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//            client.BaseAddress = new Uri(membersUrl);            
//            var result = client.GetStringAsync("").Result;                       
//            var member = JsonConvert.DeserializeObject<Member>(result);
            
//            //get the default movie
//            var defaultMovie = member.Movies.First(m=>m.Title=="Rambo");

//            //act go find it through the helper
//            var helper = new SearchHelper();
//            var movies = helper.Search(new MovieSearchCriteria { Title = "Rambo" });
//            var rambo = movies.Single(m => m.Key.NetflixId == defaultMovie.Key.NetflixId 
//                                        && m.Key.RottenTomatoesId == defaultMovie.Key.RottenTomatoesId);
//            //assert
//            CompareMovies(rambo, defaultMovie);

//        }

//        private void CompareMovies(Movie rambo, Movie defaultMovie)
//        {
//            Assert.That(rambo.Title, Is.EqualTo(defaultMovie.Title), "Titles don't match");            
//            Assert.That(rambo.Summary, Is.EqualTo(defaultMovie.Summary), "Summary didn't match");
//            Assert.That(rambo.Source, Is.EqualTo(defaultMovie.Source), "Source didn't match");
//            CollectionAssert.AreEquivalent(rambo.Cast, defaultMovie.Cast);
//        }

//        //[Test]
//        //public void AddToQueue_WhenNewMovieAddedToQueue_MovieKeyHasCorrecIds()
//        //{

//        //}

//        private string MakeUrl(string path)
//        {
//            var url = string.Format("{0}{1}", movieMonRoot, path);
//            return url;
//        }

                
//    }
//}
