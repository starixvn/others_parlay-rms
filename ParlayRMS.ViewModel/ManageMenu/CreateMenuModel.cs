// --------------------------------------------------------------------------
// <copyright file="CreateMenuModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageMenu
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Utility;

    /// <summary>
    /// Create Menu Model.
    /// </summary>
    public class CreateMenuModel
    {
        [Required, StringLength(50)]
        [Display(Name = "Name", ResourceType = typeof(ViewLang))]
        public string Name { get; set; }

        [Display(Name = "Url", ResourceType = typeof(ViewLang))]
        public string Url { get; set; }

        [StringLength(100)]
        [Display(Name = "Description", ResourceType = typeof(ViewLang))]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Function", ResourceType = typeof(ViewLang))]
        public int FunctionId { get; set; }

        [Required]
        [Display(Name = "Parent", ResourceType = typeof(ViewLang))]
        public int ParentId { get; set; }

        [Required, StringLength(50)]
        public string ParentName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required, Range(0, int.MaxValue)]
        [Display(Name = "Order", ResourceType = typeof(ViewLang))]
        public int DisplayOrder { get; set; }
    }
}