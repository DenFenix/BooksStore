using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: null,
            //    url: "Page{page}",
            //    defaults: new { controller = "Book", action="List"});

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Book", action = "List", id = UrlParameter.Optional }
            //);

            routes.MapRoute(null,
                "",
                new
                {
                    controller = "Book",
                    action = "List",
                    categoru = (string)null,
                    page = 1
                });

            routes.MapRoute(
                name:null,
                url:"Page{page}",
                defaults: new { controller = "Book", action = "List" },
                constraints: new { page = @"\d+" });

            routes.MapRoute(null, "{category}",
                new { controller = "Book", action = "List", page = 1 });

            routes.MapRoute(null,
                "{category}/Page{page}",
                new { controller = "Book", action = "List" },
                new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
