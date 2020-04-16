using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using OdeToFood.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace OdeToFood.Web
{
    public class ContainerConfig
    {
        internal static void RegisterContainer(HttpConfiguration httpConfiguration)
        {
            var builder = new ContainerBuilder();

            // first register controllers - it will get all the controllers
            // MvcApplication is the assymbly in Global.asax
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // For register API controllers
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            // Registed the type InMemoryRestaurantData whenever someone ask for IRestaurantData
            builder.RegisterType<SqlRestaurantData>()
                .As<IRestaurantData>()
                .InstancePerRequest(); // need instance per request not singleton
            builder.RegisterType<OdeToFoodDbContext>().InstancePerRequest();
            

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            httpConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}