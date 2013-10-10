// --------------------------------------------------------------------------
// <copyright file="RMS_GroupPolicySel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Groups
{
    /// <summary>
    /// Group Policy model.
    /// </summary>
    public class RMS_GroupPolicySel
    {
        public int FuncId { get; set; }

        public string FuncKey { get; set; }

        public string FuncName { get; set; }

        public int DisplayOrder { get; set; }

        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public int Policy { get; set; }
    }
}