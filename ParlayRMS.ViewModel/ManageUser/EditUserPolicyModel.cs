// --------------------------------------------------------------------------
// <copyright file="EditUserPolicyModel.cs" company="">
// Copyright (c) . All rights reserved.
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
