// --------------------------------------------------------------------------
// <copyright file="CreateExistingUserModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageUser
{
    /// <summary>
    /// Create Existing UserModel.
    /// </summary>
    public class CreateExistingUserModel
    {
        public string UserName { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }
    }
}
