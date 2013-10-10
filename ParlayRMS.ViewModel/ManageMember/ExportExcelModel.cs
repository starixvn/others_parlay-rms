// --------------------------------------------------------------------------
// <copyright file="ExportExcelModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Export Excel Model.
    /// </summary>
    public class ExportExcelModel
    {
        [Required]
        public string UserName { get; set; }

        [Range(0, 10)]
        public int? Level { get; set; }
    }
}