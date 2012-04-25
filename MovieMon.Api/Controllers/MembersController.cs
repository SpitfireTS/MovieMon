using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using MovieMon.Api.Data;
using MovieMon.Api.Models;

namespace MovieMon.Api.Controllers
{
    public class MembersController : ApiController
    {
        private readonly UrlHelperWrapper _urlHelperWrapper;
        private static IMemberRepository _memberRepo = null;
        private static ILog _logger = null;
        public UrlHelperWrapper UrlHelperWrapper { get; set; }
        
        public MembersController(ILog logger, IMemberRepository repo, UrlHelperWrapper urlHelperWrapper):base()
        {
            UrlHelperWrapper = urlHelperWrapper;

            if (_memberRepo == null)
            {
                _memberRepo = repo;
            }

            if (_logger == null)
            {
                _logger = logger;
            }
        }

        public MembersController()
            : this(LogManager.GetLogger(typeof(MembersController)), new InMemoryMemberRepo(), new UrlHelperWrapper())
        {

        }

        public IEnumerable<Member> GetAllMembers()
        {
            _logger.Info("Searching for all members...");
            var members = _memberRepo.GetAll();
            _logger.InfoFormat("Found {0}", members!=null ? members.Count() : 0) ;           
            return members;
           
        }

        public Member GetMember(Guid id)
        {
            _logger.InfoFormat("Searching for member {0} ", id);
            var member = _memberRepo.GetById(id);
            if (member==null)
            {
                _logger.ErrorFormat("Member {0} not found!", id);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return member;
        }

        public HttpResponseMessage<Member> PostMember(Member member)
        {           
            _logger.InfoFormat("Creating member {0}", member!=null ? member.Name : "name not specified");
            member = _memberRepo.Add(member);

            var response = GetMememberResponse(member, HttpStatusCode.Created);
            _logger.InfoFormat("Member: {0} was created successfully!", member.Name);
            return response;
        }

        public HttpResponseMessage PutMember(Member member)
        {
            _logger.InfoFormat("Updating member: {0}", member.Id);
            if (!_memberRepo.Update(member))
            {
                _logger.ErrorFormat("Member {0} not found!", member.Id);
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var m = _memberRepo.GetById(member.Id.Value);
            var response = GetMememberResponse(m, HttpStatusCode.NoContent);
            return response;
        }

        private HttpResponseMessage<Member> GetMememberResponse(Member member, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage<Member>(member, statusCode);
            if (UrlHelperWrapper != null)
            {                
                var uri = UrlHelperWrapper.Route("SingleMember", new {id = member.Id}, Url);
                response.Headers.Location = new Uri(Request.RequestUri, uri);
            }
            return response;
        }
        
    }
}
