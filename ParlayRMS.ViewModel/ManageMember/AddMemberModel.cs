// --------------------------------------------------------------------------
// <copyright file="AddMemberModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Utility;

    /// <summary>
    /// Add Member Model.
    /// </summary>
    public class AddMemberModel
    {
        [Required]
        [Display(Name = "Username", ResourceType = typeof(ViewLang))]
        public string UserName { get; set; }

        [Required, Range(0, 10)]
        [Display(Name = "Level", ResourceType = typeof(ViewLang))]
        public int Level { get; set; }
    }
}