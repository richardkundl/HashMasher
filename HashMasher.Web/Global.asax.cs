using System.Web.Mvc;
using System.Web.Routing;

namespace HashMasher.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             "DefaultTag", // Route name
             "{*id}", // URL with parameters
             new { controller = "Home", action = "Index" } // Parameter defaults
         );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

           

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            Bootstrapper.Run();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}