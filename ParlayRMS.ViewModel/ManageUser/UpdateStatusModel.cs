// --------------------------------------------------------------------------
// <copyright file="UpdateStatusModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageUser
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Update Status Model.
    /// </summary>
    public class UpdateStatusModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}