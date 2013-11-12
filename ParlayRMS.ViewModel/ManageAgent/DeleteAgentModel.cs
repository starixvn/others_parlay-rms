// --------------------------------------------------------------------------
// <copyright file="DeleteAgentModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.11.04">Created</change>
// </history>

namespace ParlayRMS.ViewModel.ManageAgent
{
    /// <summary>
    /// Delete agent view model.
    /// </summary>
    public class DeleteAgentModel
    {
        public string UserName { get; set; }

        public int AgentId { get; set; }
    }
}