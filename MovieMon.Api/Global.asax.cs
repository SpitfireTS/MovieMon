using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MovieMon.Api
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                "Genres",
                "api/Movies/Genres",
               new { controller = "Movies", action = "GetGenres"});

            routes.MapHttpRoute(
                "MovieList",
                "api/Movies/{name}",
               new { controller = "Movies", action = "GetByName", name = UrlParameter.Optional });

            routes.MapHttpRoute(
                "MoviesByReleaseDate",
                "api/Movies/ReleaseDate/{releaseDate}",
               new { controller = "Movies", action = "GetByReleaseDate", name=UrlParameter.Optional});

            routes.MapHttpRoute(
                "MoviesByGenre",
                "api/Movies/Genre/{genre}",
               new { controller = "Movies", action = "GetByGenre", name = UrlParameter.Optional });




            routes.MapHttpRoute(
                "SingleMember",
                "api/Members/{id}",
               new { controller = "Members", action = "GetMember", name = UrlParameter.Optional });
            
            routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();
        }
    }
}