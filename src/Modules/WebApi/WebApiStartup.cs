using System;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json.Converters;
using Owin;

namespace winctrl.Modules.WebApiStartup
{
    class WebApiStartup
    {
        private static string _baseAddress = "http://localhost:8040/";
        
        public static void Run()
        {
            // Start OWIN host 
            using (Start())
            {
                Console.WriteLine("WebApi starting at {0}", _baseAddress);
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
        
        public static IDisposable Start()
        {
            return WebApp.Start<WebApiStartup>(url: _baseAddress);
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