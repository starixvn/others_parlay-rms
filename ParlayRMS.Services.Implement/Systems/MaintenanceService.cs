// --------------------------------------------------------------------------
// <copyright file="MaintenanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Implement.Systems
{
    using ParlayRMS.Models.Systems;
    using ParlayRMS.Utility;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// Maintenance Service.
    /// </summary>
    public class MaintenanceService : BaseService
    {
        /// <summary>
        /// Gets maintenance information.
        /// </summary>
        /// <returns>Maintenance information.</returns>
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