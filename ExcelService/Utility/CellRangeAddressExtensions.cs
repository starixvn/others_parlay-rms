// --------------------------------------------------------------------------
// <copyright file="CellRangeAddressExtensions.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.08">Separated from NPOIExtensions</change>
// </history>

namespace ParlayRMS.ExcelService.Utility
{
    using NPOI.SS.Util;

    /// <summary>
    /// Cell Range Address Extensions.
    /// </summary>
    public static class CellRangeAddressExtensions
    {
        /// <summary>
        /// Clones a cell range address.
        /// </summary>
        /// <param name="range">CellRangeAddress instance.</param>
        /// <returns>New CellRangeAddress.</returns>
        public static CellRangeAddress Clone(this CellRangeAddress range)
        {
            CellRangeAddress result = new CellRangeAddress(range.FirstRow, range.LastRow, range.FirstColumn, range.LastColumn);
            return result;
        }
    }
}
