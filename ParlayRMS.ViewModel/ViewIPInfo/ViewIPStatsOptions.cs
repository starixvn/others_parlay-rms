// --------------------------------------------------------------------------
// <copyright file="ViewIPStatsOptions.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ken.Nguyen" date="2014.04.18">Created</change>
// </history>

namespace ParlayRMS.ViewModel.ViewIPInfo
{
    using System;

    /// <summary>
    /// View IP statistics Options Model.
    /// </summary>
    public class ViewIPStatsOptions
    {
        public DateTime FromDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public long MaxTransId { get; set; }
    }
}