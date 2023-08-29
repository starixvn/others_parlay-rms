namespace ParlayRMS
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web;
    using Fanex.Logging;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Logging.Sentry.RavenClient;
    using SharpRaven;

    public static class FanexLogging
    {
        public static void Register()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var sentryToken = appSettings["SentryToken"];
            var frontEndSentryToken = appSettings["FrontEndSentryToken"];
            var logCategory = appSettings["SiteCategory"];
            var logEngines = new List<ILogEngine>();
            var sentryEngineOptions = new SentryEngineOptions
            {
                Dsn = new Dsn(sentryToken),
                FrontEndDsn = new Dsn(frontEndSentryToken),
            };
            var sentryEngine = new SentryLogging(sentryEngineOptions);
            logEngines.Add(sentryEngine);

            LogManager
           .SetDefaultLogCategory(logCategory)
           .Use(logEngines);

            var configuration = new LoggingConfiguration()
                 .GetHttpHeaderInfo()
                 .GetHttpSessionInfo()
                 .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);
            ExceptionHandler.UseConfig(configuration);
        }

        public static void HandleApplicationError(HttpServerUtility server)
        {
            ExceptionHandler.Handle(server.GetLastError());
        }
    }
}