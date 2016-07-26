// --------------------------------------------------------------------------
// <copyright file="ErrorController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2014.3.31">Created</change>
// </history>

namespace ParlayRMS.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Error controller class.
    /// </summary>
    public class ErrorController : BaseController
    {
        /// <summary>
        /// Accesses the denied.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}