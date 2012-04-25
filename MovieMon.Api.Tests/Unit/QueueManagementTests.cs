using System.Linq;
using log4net;
using MovieMon.Api.Controllers;
using MovieMon.Api.Data;
using MovieMon.Api.Models;
using NUnit.Framework;

namespace MovieMon.Api.Tests.Unit
{
    [TestFixture]
    public class QueueManagementTests
    {
        [Test]
        public void AddToQueue_WhenNewMovieAddedToQueue_MovieQueueIsUpdated()
        {
            //arrange
            var controller = new MembersController(LogManager.GetLogger(typeof(MembersController)), new InMemoryMemberRepo(), null);
            var members = controller.GetAllMembers();            
            Assume.That(members.Any());
            var memberToAdd = members.First();
            
            //act
            memberToAdd.Movies.Add(new Movie
                                       {
                                           Key = new MovieKey { IsInQueue = true, NetflixId = "12345", RottenTomatoesId = "12345", wasWatched = false },
                                           Title = "My Movie"                                          
                                       });

            controller.PutMember(memberToAdd);
            //assert
            memberToAdd = controller.GetMember(memberToAdd.Id.Value);
            Assert.That(memberToAdd.Movies.Any(m=>m.Title=="My Movie"));
        }

        [Test]
        public void GetMember_WhenMemberExists_MemberIsRetrievedWithQueue()
        {
            //arrange
            var controller = new MembersController();            
            //act
            var members = controller.GetAllMembers();            

            //assert
            CollectionAssert.AllItemsAreNotNull(members.Select(m=>m.Movies));
        }

        [Test]
        public void MarkAsWatched_WhenMovieIsMarkedAsWatched_MovieQueueIsUpdated()
        {
            //arrange
            var controller = new MembersController(LogManager.GetLogger(typeof(MembersController)), new InMemoryMemberRepo(), null);
            var members = controller.GetAllMembers();
            Assume.That(members.Any());
            var memberToModify = members.First();
            
            //add a movie that hasn't been watched yet.
            memberToModify.Movies.Add(new Movie
                                       {
                                           Key = new MovieKey { IsInQueue = true, NetflixId = "12345", RottenTomatoesId = "12345", wasWatched = false },
                                           Title = "My Watched Movie"                                          
                                       });

            controller.PutMember(memberToModify);            
            memberToModify = controller.GetMember(memberToModify.Id.Value);

            // mark it as watched now and save the movie...
            var movie = memberToModify.Movies.Where(m => m.Title == "My Watched Movie").Single();
            
            Assume.That(movie.Key.wasWatched, Is.False);

            var idx = memberToModify.Movies.IndexOf(movie);
            movie.Key.wasWatched = true;
            memberToModify.Movies[idx] = movie;

            controller.PutMember(memberToModify);

            //fetch the member again
            memberToModify = controller.GetMember(memberToModify.Id.Value);

            //assert            
            Assert.That(memberToModify.Movies.Where(m => m.Title == "My Watched Movie").Select(m => m.Key.wasWatched).Single(), Is.True);
        }

        [Test]
        public void AddRating_WhenMemberRatesAMovie_MovieQueueIsUpdated()
        {
            var controller = new MembersController(LogManager.GetLogger(typeof(MembersController)), new InMemoryMemberRepo(), null);
            var members = controller.GetAllMembers();
            Assume.That(members.Any());
            var memberUnderTest = members.First();          
            memberUnderTest.Movies.Add(new Movie
                                        {
                                            Key = new MovieKey { IsInQueue = true, NetflixId = "12345", RottenTomatoesId = "12345", wasWatched = true},
                                            Title = "My Rated Movie"
                                        });

            controller.PutMember(memberUnderTest);
            var movie = memberUnderTest.Movies.Where(m => m.Title == "My Rated Movie").Single();
            Assume.That(movie.Key.Comment == null);
            Assume.That(movie.Key.Rating == null);

            //Review the movie            
            var idx = memberUnderTest.Movies.IndexOf(movie);
            movie.Key.Rating = 5;
            movie.Key.Comment = "I liked this movie";
            memberUnderTest.Movies[idx] = movie;
            controller.PutMember(memberUnderTest);

            //assert            
            var theMovie = memberUnderTest.Movies.Single(m => m.Title == "My Rated Movie");
            Assert.That(theMovie.Key.Comment == "I liked this movie");
            Assert.That(theMovie.Key.Rating.Value == 5);
        }
    }
}
