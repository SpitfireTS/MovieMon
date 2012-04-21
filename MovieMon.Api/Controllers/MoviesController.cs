using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using log4net;
using MovieMon.Api.Models;
using MovieMon.Api.Services;

namespace MovieMon.Api.Controllers
{
    
    public class MoviesController : ApiController
    {

        private static readonly ILog Logger = LogManager.GetLogger(typeof(MoviesController));
        
        // GET /api/moviesearch
       
        public HttpResponseMessage<IEnumerable<Movie>> GetByName(string name)
        {
        
           return SafeInvoke(()=>{                                        
                    Logger.InfoFormat("Received search movie request for title {0}", name);
                    var helper = new SearchHelper();
                    var movies = helper.Search(new MovieSearchCriteria {Title = name});
                    var response = new HttpResponseMessage<IEnumerable<Movie>>(movies, HttpStatusCode.OK);
                    return response;
            });
               
        }

        public HttpResponseMessage<IEnumerable<Movie>> GetByReleaseDate(DateTime releaseDate)
        {

            return SafeInvoke(() =>{
                                    Logger.InfoFormat("Received search movie request for Release Date {0}", releaseDate);
                                    var helper = new SearchHelper();
                                    var movies = helper.Search(new MovieSearchCriteria { ReleaseDate = releaseDate });
                                    var response = new HttpResponseMessage<IEnumerable<Movie>>(movies, HttpStatusCode.OK);
                                    return response;
                                });
        }

        public HttpResponseMessage<IEnumerable<Movie>> GetByGenre(string genre)
        {
            return SafeInvoke<Movie>(() =>
            {
                                    Logger.InfoFormat("Received search movie request for genre {0}", genre);
                                    var helper = new SearchHelper();
                                    var movies = helper.Search(new MovieSearchCriteria { Genre = genre });
                                    var response = new HttpResponseMessage<IEnumerable<Movie>>(movies, HttpStatusCode.OK);
                                    return response;
                                });

        }

        public HttpResponseMessage<IEnumerable<string>> GetGenres()
        {
            return SafeInvoke(() =>
                                {
                                    Logger.Info("Fetching genres...");
                                    var genres = GenreMap.GetMaps().Select(m=>m.Name).ToList();
                                    var response = new HttpResponseMessage<IEnumerable<string>>(genres, HttpStatusCode.OK);
                                    return response;
                                });

        }

        private HttpResponseMessage<IEnumerable<T>> SafeInvoke<T>(Func<HttpResponseMessage<IEnumerable<T>>> searchMethod)
        {
            try
            {
                return searchMethod();
            }
            catch (Exception e)
            {
                Logger.Error("An error occurred while processing search request.", e);
                return new HttpResponseMessage<IEnumerable<T>>(new List<T>(), HttpStatusCode.OK);
            }
        }
    }
}
