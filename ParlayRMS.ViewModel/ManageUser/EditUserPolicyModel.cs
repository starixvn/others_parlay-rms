// --------------------------------------------------------------------------
// <copyright file="EditUserPolicyModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageUser
{
    /// <summary>
    /// Edit User Policy Model.
    /// </summary>
    public class EditUserPolicyModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserPolicy { get; set; }
    }
}
