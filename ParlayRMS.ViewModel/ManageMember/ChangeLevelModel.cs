// --------------------------------------------------------------------------
// <copyright file="ChangeLevelModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Change Level Model.
    /// </summary>
    public class ChangeLevelModel
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required, Range(0, 10)]
        public int Level { get; set; }
    }
}
