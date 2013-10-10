// --------------------------------------------------------------------------
// <copyright file="EditGroupModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageGroup
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Utility;

    /// <summary>
    /// Edit Group Model.
    /// </summary>
    public class EditGroupModel
    {
        [Required]
        public int GroupId { get; set; }

        [Required]
        [Display(Name = "GroupName", ResourceType = typeof(ViewLang))]
        public string GroupName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ViewLang))]
        public string Description { get; set; }
    }
}