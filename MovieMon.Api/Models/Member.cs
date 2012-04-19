using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieMon.Api.Models
{
    public class Member
    {
        
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<MovieKey> Movies { get; set; }
        public DateTime LastModified { get; set; }
    }
}