// --------------------------------------------------------------------------
// <copyright file="Extension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Common.Models
{
    using Fanex.Data.DNAide;
    using ParlayRMS.DTO;
    using ParlayRMS.Utility;

    /// <summary>
    /// Extend DBResult functionalities.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Converts DBResult object to Result object.
        /// </summary>
        /// <param name="r">DBResult which the method expected to extend to.</param>
        /// <returns>Result object.</returns>
        public static Result ToResult(this DbResult r)
        {
            if (r.IsValid())
            {
                return new Result(DbConverter.ToInt32(r.Outputs["errcode"]), r.Outputs["errmsg"].ToString());
            }

            return Result.Empty;
        }

        /// <summary>
        /// Checks if the DBResult object is valid or not.
        /// </summary>
        /// <param name="r">DBResult which the method expected to extend to.</param>
        /// <returns>A value indicates whether or not the DBResult is valid.</returns>
        public static bool IsValid(this DbResult r)
        {
            if (r != null && r.Outputs != null && r.Outputs.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}