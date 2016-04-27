// --------------------------------------------------------------------------
// <copyright file="IndexModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.TotalOutstanding
{
    /// <summary>
    /// Index Model.
    /// </summary>
    public class IndexModel
    {
        public IndexModel()
        {
            this.Username = string.Empty;
            this.Combo = string.Empty;
            this.SportsType = string.Empty;
        }

        public string Username { get; set; }

        public string Combo { get; set; }

        public string ParlaysType { get; set; }

        public string SportsType { get; set; }
    }
}