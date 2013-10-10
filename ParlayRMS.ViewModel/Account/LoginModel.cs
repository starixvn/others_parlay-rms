// --------------------------------------------------------------------------
// <copyright file="LoginModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.Account
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Login Model.
    /// </summary>
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}