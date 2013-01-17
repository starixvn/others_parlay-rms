// -----------------------------------------------------------------------
// <copyright file="Extension.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.Common.Models
{
    using Fanex.Data.DNAide;
    using ParlayRMS.Models;
    using ParlayRMS.Utility;
    using ParlayRMS.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Extension
    {
        public static Result ToResult(this DbResult r)
        {
            if (r.IsValid())
            {
                return new Result(DbConverter.ToInt32(r.Outputs["errcode"]), r.Outputs["errmsg"].ToString());
            }

            return Result.Empty;
        }

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