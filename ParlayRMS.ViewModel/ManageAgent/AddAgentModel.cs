// --------------------------------------------------------------------------
// <copyright file="AddAgentModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.11.04">Created</change>
// </history>

namespace ParlayRMS.ViewModel.ManageAgent
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Utility;

    /// <summary>
    /// Add agent model.
    /// </summary>
    public class AddAgentModel
    {
        [Required]
        [Display(Name = "Username", ResourceType = typeof(ViewLang))]
        public string UserName { get; set; }
    }
}