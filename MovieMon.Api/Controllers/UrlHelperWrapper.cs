using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;

namespace MovieMon.Api.Controllers
{
    public class UrlHelperWrapper
    {
        public string Route(string routeName, object o, UrlHelper url)
        {
            return url.Route(routeName, o);
        }
    }
}