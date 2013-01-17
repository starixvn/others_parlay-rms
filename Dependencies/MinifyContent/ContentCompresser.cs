using System;
using System.Reflection;
using System.Web;

namespace MinifyContent
{
    class ContentCompresser : IHttpModule
    {

        static bool? isDebugMode;

        public void Dispose()
        {}

        public void Init(HttpApplication context)
        {
            //Handle Post Release Request to handle page output
            context.PostReleaseRequestState += new EventHandler(context_PostReleaseRequestState);
        }

        protected void context_PostReleaseRequestState(object sender, EventArgs e)
        {

            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;

            //set isDebugMode if its value is null
            if (isDebugMode == null)
            { 
                Assembly assembly = AssemblyGen.GetWebApplicationAssembly(context);
                isDebugMode = AssemblyGen.IsDebugMode(assembly);
            }
            //Check that running web assembly is not in debug mode and also check that Content type should be "text/html" or "text/css"
            /* Two If Conditions are here, first is in use and second is commented. 
             * For published mode use first condition. This will make assurance that your deployment has been done in publish mode.
             * To verify At development time for unit testing purpose use second condition in debug & release mode.
             */
            if (!isDebugMode.Value && !context.IsDebuggingEnabled && (context.Response.ContentType == "text/html" || context.Response.ContentType == "text/css"))
            //if (!isDebugMode.Value && (context.Response.ContentType == "text/html" || context.Response.ContentType == "text/css"))
                context.Response.Filter = new ResponseStream(context.Response);

        }

    }
}
