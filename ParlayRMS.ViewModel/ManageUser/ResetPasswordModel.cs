// -----------------------------------------------------------------------
// <copyright file="LoginModel.cs" company="">
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
    public class ResetPasswordModel
    {
        [Required]
        public int UserId { get; set; }

        [Display(Name = "Username", ResourceType = typeof(ViewLang))]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmpassword", ResourceType = typeof(ViewLang))]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Newpassword", ResourceType = typeof(ViewLang))]
        public string NewPassword { get; set; }
    }
}