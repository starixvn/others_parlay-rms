// -----------------------------------------------------------------------
// <copyright file="UserModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.ManageUser
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using ParlayRMS.Utility;
    using ParlayRMS.ViewModel.Extension;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditUserModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Username", ResourceType = typeof(ViewLang))]
        public string UserName { get; set; }

        [Display(Name = "Firstname", ResourceType = typeof(ViewLang))]
        public string FirstName { get; set; }

        [Display(Name = "Lastname", ResourceType = typeof(ViewLang))]
        public string LastName { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Display(Name = "Groupname", ResourceType = typeof(ViewLang))]
        public string GroupName { get; set; }
    }
}