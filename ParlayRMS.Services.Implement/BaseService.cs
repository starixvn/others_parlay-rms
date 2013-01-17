namespace ParlayRMS.Services.Implement
{
    using System;
    using Fanex.Logging;
    using System.Configuration;

    public abstract class BaseService
    {
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