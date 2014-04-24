// --------------------------------------------------------------------------
// <copyright file="RMS_Rpt_Parlay_ViewIPInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.18">Created</change>
// </history>

namespace ParlayRMS.DTO.ViewIPInfo
{
    /// <summary>
    /// View IP Info model.
    /// </summary>
    public class RMS_Rpt_Parlay_ViewIPInfo
    {
        public string Username { get; set; }

        public int UserId { get; set; }

        public int Soccer { get; set; }

        public int Others { get; set; }

        public int Total { get; set; }
    }
}