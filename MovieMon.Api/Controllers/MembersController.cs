using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using MovieMon.Api.Data;
using MovieMon.Api.Models;

namespace MovieMon.Api.Controllers
{
    public class MembersController : ApiController
    {
        static readonly IMemberRepository _memberRepo = new InMemoryMemberRepo();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MembersController));

        public IEnumerable<Member> GetAllMembers()
        {
            Logger.Info("Searching for all members...");
            var members = _memberRepo.GetAll();
            Logger.InfoFormat("Found {0}", members!=null ? members.Count() : 0) ;           
            return members;
           
        }

        public Member GetMember(Guid id)
        {
            Logger.InfoFormat("Searching for member {0} ", id);
            var member = _memberRepo.GetById(id);
            if (member==null)
            {
                Logger.ErrorFormat("Member {0} not found!", id);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return member;
        }

        public HttpResponseMessage<Member> PostMember(Member member)
        {           
            Logger.InfoFormat("Creating member {0}", member!=null ? member.Name : "name not specified");
            member = _memberRepo.Add(member);

            var response = GetMememberResponse(member, HttpStatusCode.Created);
            Logger.InfoFormat("Member: {0} was created successfully!", member.Name);
            return response;
        }

        private HttpResponseMessage<Member> GetMememberResponse(Member member, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage<Member>(member, statusCode);
            var uri = Url.Route("SingleMember", new {id = member.Id});
            response.Headers.Location = new Uri(Request.RequestUri, uri);
            return response;
        }

        public HttpResponseMessage PutMember(Member member)
        {
            Logger.InfoFormat("Updating member: {0}", member.Id);
            if (!_memberRepo.Update(member))
            {
                Logger.ErrorFormat("Member {0} not found!", member.Id);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var response = new HttpResponseMessage(HttpStatusCode.NoContent);
            var uri = Url.Route("SingleMember", new { id = member.Id });
            response.Headers.Location = new Uri(Request.RequestUri, uri);

            return response;
        }
        
        
    }
}
