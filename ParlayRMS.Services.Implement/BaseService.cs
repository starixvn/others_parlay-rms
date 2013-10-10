// --------------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement
{
    using System;
    using System.Configuration;
    using Fanex.Logging;

    /// <summary>
    /// Base Service.
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// Processes Exception.
        /// </summary>
        /// <param name="ex">Occurred Exception.</param>
        /// <param name="category">Category of Exception.</param>
        protected void ProcessException(Exception ex, string category = "PARLAY_RMS")
        {
            LogService.Error(ex.Message, category, ex);

            if (ConfigurationManager.AppSettings["IsDebug"].Equals(bool.TrueString))
            {
                throw ex;
            }
        }
    }
}