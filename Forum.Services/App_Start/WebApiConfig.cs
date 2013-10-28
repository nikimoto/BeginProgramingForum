using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Forum.Services
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
               name: "PostsApi",
               routeTemplate: "api/posts/{action}/{id}",
               defaults: new { controller = "posts", id = RouteParameter.Optional }
           );

             config.Routes.MapHttpRoute(
                name: "UsersApi",
                routeTemplate: "api/users/{action}",
                defaults: new { controller = "users"}
            );

             config.Routes.MapHttpRoute(
                 name: "ModifiedApi",
                 routeTemplate: "api/{controller}/{action}",
                 defaults: new { }
             );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
