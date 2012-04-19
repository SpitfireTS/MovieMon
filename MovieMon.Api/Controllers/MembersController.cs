using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MovieMon.Api.Data;
using MovieMon.Api.Models;

namespace MovieMon.Api.Controllers
{
    public class MembersController : ApiController
    {
        static readonly IMemberRepository _memberRepo = new InMemoryMemberRepo();

        public IEnumerable<Member> GetAllMembers()
        {
            return _memberRepo.GetAll();
        }

        public Member GetMember(Guid id)
        {
            var member = _memberRepo.GetById(id);
            if (member==null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return member;
        }

        public HttpResponseMessage<Member> PostMember(Member member)
        {           
            member = _memberRepo.Add(member);
            var response = new HttpResponseMessage<Member>(member, HttpStatusCode.Created);
            var uri = Url.Route(null, new {id = member.Id});
            response.Headers.Location = new Uri(Request.RequestUri, uri);
            return response;
        }

        public void PutMember(Member member)
        {
            if (!_memberRepo.Update(member))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
