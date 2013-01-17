namespace ParlayRMS.Services.Implement.Systems
{
    using ParlayRMS.Models;
    using ParlayRMS.Utility;
    using Sunplus.UMControllerLibrary;
    using ParlayRMS.Models.Systems;

    public class MaintenanceService : BaseService
    {
        public MaintenanceInfo GetMaintenanceInfo()
        {
            var maintenanceInfo = new MaintenanceInfo { IsDownTime = false };

            if (AppSetting.IsUM)
            {
                maintenanceInfo.IsDownTime = true;
                maintenanceInfo.TimeFrom = AppSetting.TimeFrom;
                maintenanceInfo.TimeTo = AppSetting.TimeTo;
            }
            else
            {
                var service = new UMBusiness();
                var timeFrom = maintenanceInfo.TimeFrom;
                var timeTo = maintenanceInfo.TimeTo;
                maintenanceInfo.IsDownTime = service.isUnderMaintainence(Sites.Agency, AppSetting.LBUrl, ref timeFrom, ref timeTo);
                if (maintenanceInfo.IsDownTime)
                {
                    maintenanceInfo.TimeFrom = timeFrom;
                    maintenanceInfo.TimeTo = timeTo;
                }
            }

            return maintenanceInfo;
        }
    }
}