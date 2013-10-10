// --------------------------------------------------------------------------
// <copyright file="EditUserModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageUser
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Utility;

    /// <summary>
    /// Edit User Model.
    /// </summary>
    public class EditUserModel
    {
        [Required]
        public int UserId { get; set; }

        [Display(Name = "Username", ResourceType = typeof(ViewLang))]
        public string UserName { get; set; }

        [Display(Name = "Firstname", ResourceType = typeof(ViewLang))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string FirstName { get; set; }

        [Display(Name = "Lastname", ResourceType = typeof(ViewLang))]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "GroupName", ResourceType = typeof(ViewLang))]
        public int GroupId { get; set; }

        public string GroupName { get; set; }
    }
}