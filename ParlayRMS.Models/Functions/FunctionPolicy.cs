// --------------------------------------------------------------------------
// <copyright file="FunctionPolicy.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Functions
{
    using ParlayRMS.Models;

    /// <summary>
    /// Function Policy Model.
    /// </summary>
    public class FunctionPolicy
    {
        public int FunctionId { get; set; }

        public string FunctionName { get; set; }

        public string FunctionKey { get; set; }

        public Policy Policy { get; set; }
    }
}