// --------------------------------------------------------------------------
// <copyright file="BetDetailsOptions.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Report
{
    /// <summary>
    /// Bet Details Options.
    /// </summary>
    public class BetDetailsOptions
    {
        public long TransId { get; set; }

        public long OriginalTransId { get; set; }

        public long RefNo { get; set; }

        public long OriginalRefNo { get; set; }

        public string Username { get; set; }
    }
}