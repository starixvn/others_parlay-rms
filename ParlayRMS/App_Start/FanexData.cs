namespace ParlayRMS
{
    using Fanex.Data;
    using Fanex.Data.SqlServer;

    public static class FanexData
    {
        public static void Register()
        {
            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer()
                .UseDefaultDbSettingProvider(resourcePath: "~/App_Data/Fanex.Data")
                .UseDefaultConnectionStringProvider()
                .Run();

        }
    }
}
