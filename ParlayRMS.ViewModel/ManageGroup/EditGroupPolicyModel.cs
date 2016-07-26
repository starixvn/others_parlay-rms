// --------------------------------------------------------------------------
// <copyright file="EditGroupPolicyModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageGroup
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Edit Group Policy Model.
    /// </summary>
    public class EditGroupPolicyModel
    {
        [Required]
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        [Required]
        public string GroupPolicy { get; set; }
    }
}