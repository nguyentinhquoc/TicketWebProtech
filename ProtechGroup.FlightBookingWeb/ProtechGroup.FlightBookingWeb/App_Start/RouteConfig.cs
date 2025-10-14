using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProtechGroup.FlightBookingWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Timkiemchuyenbay",
                url: "Timkiemchuyenbay/{data}",
                defaults: new { controller = "SearchFlight", action = "ResultsFlightDomestic", data = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Timkiemsanbay",
                url: "Timkiemsanbay/{data}",
                defaults: new { controller = "Airport", action = "SearchByKey", data = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Trangchu",
               url: "Trangchu",
               defaults: new { controller = "Home", action = "Index" }
           );
           routes.MapRoute(
               name: "Encryptedquery",
               url: "Encryptedquery",
               defaults: new { controller = "Base", action = "EncryptQuery" }
           );
           routes.MapRoute(
              name: "Cheksearchflightinput",
              url: "Cheksearchflightinput",
              defaults: new { controller = "Airport", action = "Cheksearchflightinput" }
            );
            routes.MapRoute(
              name: "Ketquatimkiem",
              url: "Ketquatimkiem/{data}",
              defaults: new { controller = "SearchFlight", action = "ResultsFlightDomestic", data = UrlParameter.Optional }
          );
            routes.MapRoute(
                 name: "Default",
                 url: "",
                 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
             );
        }
    }
}
