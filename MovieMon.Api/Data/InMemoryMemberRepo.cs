using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieMon.Api.Models;

namespace MovieMon.Api.Data
{
    public class InMemoryMemberRepo:IMemberRepository
    {
        private static IList<Member> _members = new List<Member>
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
                                                                               Title = "Rambo",
                                                                               Key = new MovieKey{NetflixId = "VoD1y", RottenTomatoesId = "770709520", wasWatched = false, IsInQueue = true}
                                                                           }
                                                                    }
                                                            },
                                                    };

        public IEnumerable<Member> GetAll()
        {            
            return _members;
        }

        public Member GetById(Guid id)
        {
            return _members.FirstOrDefault(m => m.Id == id);
        }

        public Member Add(Member member)
        {
            if (!member.Id.HasValue)
            {
                member.Id = Guid.NewGuid();
            }
            
            if (member.Movies==null)
            {
                member.Movies = new List<Movie>();
            }
            _members.Add(member);

            return member;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Member member)
        {
            var found = _members.FirstOrDefault(m => m.Id == member.Id);
            if (found!=null)
            {
                var index = _members.IndexOf(found);
                _members[index] = member;
            }
            return true;
        }
    }
}