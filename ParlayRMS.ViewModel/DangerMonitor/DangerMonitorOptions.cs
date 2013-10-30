// --------------------------------------------------------------------------
// <copyright file="DangerMonitorOptions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.23">Created</change>
// </history>

namespace ParlayRMS.ViewModel.DangerMonitor
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// Options for danger monitor filter form.
    /// </summary>
    public class DangerMonitorOptions
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [AllowHtml]
        public string AgentIdList { get; set; }

        public string Combos { get; set; }

        public bool IsFromConfigList { get; set; }
    }
}
