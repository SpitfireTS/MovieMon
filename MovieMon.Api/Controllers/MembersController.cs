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
using MovieMon.Api.Services;

namespace MovieMon.Api.Controllers
{
    public class MembersController : ApiController
    {
        private static IMemberRepository _memberRepo = null;
        private static ILog _logger = null;
        public UrlHelperWrapper UrlHelperWrapper { get; set; }
        private static List<Member> _defaultMembers;
        public MembersController(ILog logger, IMemberRepository repo, UrlHelperWrapper urlHelperWrapper)
            : base()
        {
            UrlHelperWrapper = urlHelperWrapper;

            if (_memberRepo == null)
            {
                _memberRepo = repo;
                _defaultMembers = repo.GetAll().ToList();
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
            var all = _memberRepo.GetAll();
            var members = new List<Member>();
            try
            {
                foreach (var member in all)
                {
                    members.Add(GetMemberById(member.Id.Value));
                }
            }
            catch (Exception e)
            {
                members = _defaultMembers;
                _logger.Error(e);
            }                                
            
            return members;
           
        }

        private Member GetMemberById(Guid id)
        {
            _logger.InfoFormat("Searching for member {0} ", id);
            var member = _memberRepo.GetById(id);
            var movies = GetMovies(member);
            member.Movies = movies;
            return member;
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
            var movies = GetMovies(member);
            member.Movies = movies;
            return member;
        }

        private List<Movie> GetMovies(Member member)
        {
            var helper = new SearchHelper();
            
            var movies = new List<Movie>();
            var memberMovies = member.Movies;
            foreach (var movie in memberMovies)
            {
                var criteria = new MovieSearchCriteria {Key = movie.Key};
                var movieDetail = helper.Search(criteria).FirstOrDefault();
                if (movieDetail!=null)
                {
                    //make sure we set the key to the one we already have....otherwise a new one will get created.
                    movieDetail.Key = movie.Key;
                    movies.Add(movieDetail);
                }
            }
            return movies;
        }

        public HttpResponseMessage<Member> PostMember(Member member)
        {           
            _logger.InfoFormat("Creating member {0}", member!=null ? member.Name : "name not specified");
            member = _memberRepo.Add(member);

            var response = GetMememberResponse(member, HttpStatusCode.Created);
            _logger.InfoFormat("Member: {0} was created successfully!", member.Name);
            return response;
        }

        public HttpResponseMessage<Member> PutMember(Member member)
        {
            var response = GetMememberResponse(member, HttpStatusCode.OK);
            
            _logger.InfoFormat("Updating member: {0}", member.Id);
            try
            {
                if (!_memberRepo.Update(member))
                {
                    _logger.ErrorFormat("Member {0} not found!", member.Id);
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            
                var m = _memberRepo.GetById(member.Id.Value);
                response = GetMememberResponse(m, HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                _logger.Error(e);
                if (!member.Id.HasValue)
                {
                    response = GetMememberResponse(_defaultMembers.First(), HttpStatusCode.OK);
                }
            }            
            return response;
        }

        private HttpResponseMessage<Member> GetMememberResponse(Member member, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage<Member>(member, statusCode);
            return response;
        }
        
    }
}
