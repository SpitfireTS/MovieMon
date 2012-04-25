using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using log4net;
using MovieMon.Api.Controllers;
using MovieMon.Api.Data;
using MovieMon.Api.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MovieMon.Api.Tests.Integration
{
    [TestFixture]
    public class QueueManagementTests
    {
        private string movieMonRoot = null;

        [TestFixtureSetUp]
        public void init()
        {
            //set the environment to the right root url.  this needs to be like this
            //due to App Harbor's conventions. see: http://support.appharbor.com/kb/getting-started/managing-environments

            var env = ConfigurationManager.AppSettings["Environment"];
            if (env=="Debug" || env=="Test")
            {
                movieMonRoot = "http://ipv4.fiddler/MovieMon/api/";
            }else
            {
                movieMonRoot = "http://movieman.apphb.com/api/";
            }
        }

        [Test]
        public void GetMembers_WhenInvoked_RetrievesDefaultMemember()
        {
            //arrange
            string membersUrl = MakeUrl("Members");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(membersUrl);

            //act
            var result = client.GetStringAsync("").Result;
            
            //assert
            var members = JsonConvert.DeserializeObject<IEnumerable<Member>>(result);
            var mmMember = members.FirstOrDefault(m => m.Name == "MovieMonFan");
            Assert.That(mmMember!=null);
        }

        //[Test]
        //public void GetMember_WhenInvoked_MovieRamboIsFetchedFromProviders()
        //{
        //    //arrange
        //    var controller = new MembersController();
        //    //act
        //    var members = controller.GetAllMembers();

        //    //assert
        //    CollectionAssert.AllItemsAreNotNull(members.Select(m => m.Movies));
        //}

        //[Test]
        //public void AddToQueue_WhenNewMovieAddedToQueue_MovieKeyHasCorrecIds()
        //{

        //}

        private string MakeUrl(string path)
        {
            var url = string.Format("{0}{1}", movieMonRoot, path);
            return url;
        }

                
    }
}
