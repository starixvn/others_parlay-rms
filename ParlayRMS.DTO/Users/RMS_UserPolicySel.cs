// --------------------------------------------------------------------------
// <copyright file="RMS_UserPolicySel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Users
{
    /// <summary>
    /// User Policy model.
    /// </summary>
    public class RMS_UserPolicySel
    {
        public int FuncId { get; set; }

        public string FuncKey { get; set; }

        public string FuncName { get; set; }

        public int DisplayOrder { get; set; }

        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public int UserPolicy { get; set; }

        public int GroupPolicy { get; set; }
    }
}