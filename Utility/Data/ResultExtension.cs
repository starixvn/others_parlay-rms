namespace ParlayRMS.Utility.Data
{
    using DTO;
    using Fanex.Data.DNAide;
    using ParlayRMS.Utility;

    public static class ResultExtension
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