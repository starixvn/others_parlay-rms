// --------------------------------------------------------------------------
// <copyright file="CreateFunctionModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageFunction
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Utility;

    /// <summary>
    /// Create Function Model.
    /// </summary>
    public class CreateFunctionModel
    {
        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewLang))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Key", ResourceType = typeof(ViewLang))]
        public string Key { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ViewLang))]
        public string Description { get; set; }

        [Required, Range(0, int.MaxValue)]
        [Display(Name = "Order", ResourceType = typeof(ViewLang))]
        public int DisplayOrder { get; set; }

        [Required]
        [Display(Name = "Parent", ResourceType = typeof(ViewLang))]
        public int ParentId { get; set; }

        [Required]
        public string ParentName { get; set; }
    }
}