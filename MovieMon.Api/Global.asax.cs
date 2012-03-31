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

            //routes.MapRoute(
            //  "SingleMovie",
            //  "api/Movies/{id}",
            //  new { controller = "Movies", action = "GetById", id=UrlParameter.Optional});

            routes.MapHttpRoute(
                "MovieList",
                "api/Movies/{name}",
               new { controller = "Movies", action = "GetByName", name=UrlParameter.Optional});

            routes.MapHttpRoute(
                name: "API Default",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //routes.MapRoute(
            //  "CustomerOrders",
            //  "Customers/{customerId}/Orders/{orderId}",
            //  new
            //  {
            //      controller = "Customers",
            //      action = "SingleCustomerOrders",
            //      orderId = UrlParameter.Optional
            //  });
            //routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

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