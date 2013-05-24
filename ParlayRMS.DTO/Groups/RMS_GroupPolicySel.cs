// -----------------------------------------------------------------------
// <copyright file="RMS_GroupPolicySel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.Groups
{
    /// <summary>
    /// TODO: Update summary.
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