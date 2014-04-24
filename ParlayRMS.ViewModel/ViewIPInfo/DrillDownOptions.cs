// --------------------------------------------------------------------------
// <copyright file="ViewIPInfoOptions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.18">Created</change>
// </history>

namespace ParlayRMS.ViewModel.ViewIPInfo
{
    using System;

    /// <summary>
    /// View IPInfo Drill Down Options Model.
    /// </summary>
    public class DrillDownOptions
    {
        public string IP { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public int SportType { get; set; }
    }
}