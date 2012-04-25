﻿using System;
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
                                                                Email = "dannysemail",
                                                                Id = Guid.NewGuid(), 
                                                                Movies = new List<Movie>
                                                                    {
                                                                       new Movie
                                                                           {
                                                                               Title = "The Godfather",
                                                                               Key = new MovieKey{NetflixId = "93993", RottenTomatoesId = "993343", wasWatched = false, IsInQueue = true}
                                                                           }
                                                                    }
                                                            },
                                                        new Member
                                                            {
                                                                Name    = "Todd",
                                                                Email = "Toods-email",
                                                                Id = Guid.NewGuid(),
                                                                Movies = new List<Movie>()
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