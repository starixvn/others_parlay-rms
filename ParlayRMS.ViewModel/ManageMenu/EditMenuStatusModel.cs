// --------------------------------------------------------------------------
// <copyright file="EditMenuStatusModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageMenu
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Edit Menu Status Model.
    /// </summary>
    public class EditMenuStatusModel
    {
        [Required]
        public int MenuId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}