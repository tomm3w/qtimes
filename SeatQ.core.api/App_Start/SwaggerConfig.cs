using System.Web.Http;
using SeatQ.core.api;
using WebActivatorEx;
using Swashbuckle.Application;
using System;


[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace SeatQ.core.api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            Swashbuckle.Bootstrapper.Init(GlobalConfiguration.Configuration);
        


            // NOTE: If you want to customize the generated swagger or UI, use SwaggerSpecConfig and/or SwaggerUiConfig here ...

            SwaggerUiConfig.Customize(c =>
            {
                c.InjectStylesheet(typeof(SwaggerConfig).Assembly, @"..\..\Content\swashbuckle.css");
                //c.InjectJavaScript(typeof(SwaggerConfig).Assembly, @"\Content\swashbuckle.js");
            });

            SwaggerSpecConfig.Customize(c =>
            {
                c.IncludeXmlComments(GetXmlCommentsPath());
            });
        }

        private static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}bin\SeatQ.core.api.XML", AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}