using System;
using System.Diagnostics;
using System.Reflection;
using System.Web;


namespace MinifyContent
{
    /// <summary>
    /// Class to get application assembly information 
    /// </summary>
    class AssemblyGen
    {
        private const string AspNamespace = "ASP";
        private static bool bDebugMode;

        /// <summary>
        /// Method will return the web application assembly 
        /// </summary>
        /// <param name="context">current web application context</param>
        /// <returns>current web application assembly</returns>
        public static Assembly GetWebApplicationAssembly(HttpContext context)
        {
            //Get the current handler
            Type type = context.CurrentHandler.GetType();

            //Get the current executing assembly
            while (type != null && type != typeof(object) && type.Namespace == AspNamespace)
                type = type.BaseType;

            return type.Assembly;
        }

        /// <summary>
        /// Method will check the application execution mode is debug or not. Application can be win form or web app.
        /// </summary>
        /// <param name="assembly">Assembly for which execution mode needs to check.</param>
        /// <returns>current assembly's debug mode</returns>
        public static bool IsDebugMode(Assembly assembly)
        {
            //Get the debug attribute
            object[] attribs = assembly.GetCustomAttributes(typeof(DebuggableAttribute), true);
            //by default it is debug mode
            if (attribs == null || attribs.Length == 0)
                bDebugMode = true;
            else
            {
                //if debug attribute is found then check debug mode
                var d = (DebuggableAttribute)attribs[0];
                bDebugMode = d.IsJITTrackingEnabled;
            }

            return bDebugMode;
        }
    }
}
