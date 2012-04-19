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
                                                                Name = "Danny",
                                                                Email = "dannydakjfd;jd",
                                                                Id = Guid.NewGuid(), Movies = new List<MovieKey>
                                                                    {
                                                                        new MovieKey{NetflixId = "93993", RottenTomatoesId = "993343", wasWatched = false, IsInQueue = true}
                                                                    }
                                                            },
                                                        new Member
                                                            {
                                                                Name    = "Todd",
                                                                Email = "Isuckmythumb-email",
                                                                Id = Guid.NewGuid()
                                                            }
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
            if (member.Id.HasValue && member.Id.Value!=new Guid())
            {
                member.Id = Guid.NewGuid();
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
            return true;
        }
    }
}