using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MovieMon.Api.Models;

namespace MovieMon.Api.Data
{
    public class MemberRepository : IMemberRepository
    {
        private MongoServer _server;
        private MongoDatabase _database;
        private MongoCollection<Member> _members;

        public MemberRepository(string connection)
        {
            if (string.IsNullOrWhiteSpace(connection))
            {
                connection = "mongodb://localhost:27017";
            }

            _server = MongoServer.Create(connection);
            _database = _server.GetDatabase("Members", SafeMode.True);
            _members = _database.GetCollection<Member>("Members");

            // Reset database and add some default entries            
            for (int index = 1; index < 5; index++)
            {
                var user = new Member
                               {
                                   Email = string.Format("test{0}@example.com", index),
                                   Name = string.Format("test{0}", index),
                                   Phone = string.Format("{0}{0}{0} {0}{0}{0} {0}{0}{0}{0}", index),                                                                 
                                   //Movies = new List<Movie> {new Movie() {Title = string.Format("My Flick - {0}", index)}}                                    
                                };
                Add(user);
            }
        }

        #region IUserRepository Members

        public IEnumerable<Member> GetAll()
        {
            return _members.FindAll();
        }

        public Member GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Member GetById(string id)
        {
            IMongoQuery query = Query.EQ("_id", id);
            return _members.Find(query).FirstOrDefault();
        }

        public Member Add(Member member)
        {
            
            
            _members.Insert(member);
            return member;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Member member)
        {
            var id = member.Id;
            IMongoQuery query = Query.EQ("_id", id);
            
            var movies = new BsonDocument(true);

          //  movies.Add(member.Movies.ToDictionary(movie => movie.id));
            
            IMongoUpdate update = MongoDB.Driver.Builders.Update
                .Set("Email", member.Email)
                .Set("LastModified", DateTime.UtcNow)
                .Set("Name", member.Name)
                .Set("MovieQueues", movies)
                .Set("Phone", member.Phone);
            var result = _members.Update(query, update);
            return result.UpdatedExisting;
        }

        #endregion
    }
}