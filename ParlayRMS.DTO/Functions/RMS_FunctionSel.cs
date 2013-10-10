// --------------------------------------------------------------------------
// <copyright file="RMS_FunctionSel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Functions
{
    using System;

    /// <summary>
    /// Function model.
    /// </summary>
    public class RMS_FunctionSel
    {
        public int FuncId { get; set; }

        public string FuncName { get; set; }

        public string FuncKey { get; set; }

        public string FuncDesc { get; set; }

        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}