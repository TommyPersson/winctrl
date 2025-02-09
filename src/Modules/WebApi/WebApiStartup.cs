using System;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Converters;
using Owin;

namespace winctrl.Modules.WebApiStartup
{
    class WebApiStartup
    {
        public static void Run()
        {
            string baseAddress = "http://localhost:8040/";

            // Start OWIN host 
            using (WebApp.Start<WebApiStartup>(url: baseAddress))
            {
                Console.WriteLine("WebApi starting at {0}", baseAddress);
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }

        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            
            appBuilder.UseWebApi(config);
        }
    }
}