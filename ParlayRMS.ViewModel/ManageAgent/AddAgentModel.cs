// --------------------------------------------------------------------------
// <copyright file="AddAgentModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.11.04">Created</change>
// </history>

namespace ParlayRMS.ViewModel.ManageAgent
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using ParlayRMS.Utility;

    /// <summary>
    /// Add agent model.
    /// </summary>
    public class AddAgentModel
    {
        [Required]
        [Display(Name = "Username", ResourceType = typeof(ViewLang))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [AllowHtml]
        public string UserName { get; set; }
    }
}