// --------------------------------------------------------------------------
// <copyright file="AgentListModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.11.04">Created</change>
// </history>

namespace ParlayRMS.ViewModel.ManageAgent
{
    using System.Web.Mvc;

    /// <summary>
    /// Agent list view model.
    /// </summary>
    public class AgentListModel
    {
        [AllowHtml]
        public string UserName { get; set; }

        public int? Page { get; set; }
    }
}