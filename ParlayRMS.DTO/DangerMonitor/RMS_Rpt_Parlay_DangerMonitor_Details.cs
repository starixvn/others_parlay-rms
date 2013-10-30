// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_DangerMonitor_Details.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.23">Created</change>
// </history>

namespace ParlayRMS.DTO.DangerMonitor
{
    /// <summary>
    /// The model that represents the bet details of a dangerous member.
    /// </summary>
    public class RMS_Rpt_Parlay_DangerMonitor_Details
    {
        public decimal Hdp { get; set; }

        public decimal Odds { get; set; }

        public int HdpHome { get; set; }

        public int HdpAway { get; set; }

        public int Hdp1stHome { get; set; }

        public int Hdp1stAway { get; set; }

        public int OUHome { get; set; }

        public int OUAway { get; set; }

        public int OU1stHome { get; set; }

        public int OU1stAway { get; set; }
    }
}