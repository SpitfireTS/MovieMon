using System;
using System.Collections.Generic;
using System.Linq;
using MovieMon.Api.Models;
using log4net;

namespace MovieMon.Api.Data
{
    public class InMemoryMemberRepo : IMemberRepository
    {
        private static readonly IList<Member> _members;
        private static ILog _logger = LogManager.GetLogger(typeof (InMemoryMemberRepo));
        static InMemoryMemberRepo()
        {
            _members = new List<Member>
                           {
                               new Member
                                   {
                                       Name = "MovieMonFan",
                                       Email = "MovieMonFans@moviemon-1234.com",
                                       LastName = "Fanatico",
                                       Id = new Guid("f98b9048-1324-440f-802f-ebcfab1c5395"),
                                       Phone = "999-888-7777",
                                       Movies = new List<Movie>
                                                    {
                                                        new Movie
                                                            {
                                                                Title = "Rocky",
                                                                Key = new MovieKey
                                                                          {
                                                                              NetflixId = "82Q3",
                                                                              RottenTomatoesId = "11405",
                                                                              wasWatched = false,
                                                                              IsInQueue = true
                                                                          }
                                                            },
                                                        new Movie
                                                            {
                                                                Title = "The Godfather",
                                                                Key = new MovieKey
                                                                          {
                                                                              NetflixId = "ApOD2",
                                                                              RottenTomatoesId = "12911",
                                                                              wasWatched = true,
                                                                              IsInQueue = true,
                                                                              Comment =
                                                                                  "This was a great movie. In scene after scene ... Coppola crafted an enduring, undisputed masterpiece",
                                                                              Rating = 4,
                                                                              WatchedDate = DateTime.Now.AddDays(-10).ToShortDateString()
                                                                              
                                                                          }
                                                            }
                                                    }
                                   },
                           };
        }

        #region IMemberRepository Members

        public IEnumerable<Member> GetAll()
        {
            return _members;
        }

        public Member GetById(Guid id)
        {
            Member member = _members.FirstOrDefault(m => m.Id == id);
            return member;
        }

        public Member Add(Member member)
        {
            try
            {
                if (!member.Id.HasValue)
                {
                    member.Id = Guid.NewGuid();
                }

                if (member.Movies == null)
                {
                    member.Movies = new List<Movie>();
                }
                _members.Add(member);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            return member;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Member member)
        {
            Member found = null;
            try
            {
                var movies = member.Movies.Distinct(new MovieKeyComparer());
                
                foreach (var movie in movies)
                {
                    if (movie.Key!=null && movie.Key.wasWatched)
                    {
                        movie.Key.WatchedDate = DateTime.Now.ToShortDateString();
                    }
                }

                found = _members.FirstOrDefault(m => m.Id == member.Id);
                found.Movies = movies.ToList();
                if (found != null)
                {

                    int index = _members.IndexOf(found);
                    _members[index] = member;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }
            return true;
        }

        #endregion
    }
}