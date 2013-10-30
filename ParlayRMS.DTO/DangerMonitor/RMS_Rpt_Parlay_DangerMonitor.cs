// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_DangerMonitor.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.23">Created</change>
// </history>

namespace ParlayRMS.DTO.DangerMonitor
{
    /// <summary>
    /// Model that represent general data of a dangerous member.
    /// </summary>
    public class RMS_Rpt_Parlay_DangerMonitor
    {
        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public int HdpHome { get; set; }

        public int HdpAway { get; set; }

        public int Hdp1stHome { get; set; }

        public int Hdp1stAway { get; set; }

        public int OUHome { get; set; }

        public int OUAway { get; set; }

        public int OU1stHome { get; set; }

        public int OU1stAway { get; set; }

        public string LeagueName { get; set; }

        public string MatchName { get; set; }
    }
}