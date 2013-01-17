// -----------------------------------------------------------------------
// <copyright file="RMS_FunctionSel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.Functions
{
    using System;

    /// <summary>
    /// TODO: Update summary.
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