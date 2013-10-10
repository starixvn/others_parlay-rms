// --------------------------------------------------------------------------
// <copyright file="UpdateLevelModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Update Level Model.
    /// </summary>
    public class UpdateLevelModel
    {
        [Required]
        public int CustId { get; set; }

        [Required, Range(0, 10)]
        public int Level { get; set; }
    }
}