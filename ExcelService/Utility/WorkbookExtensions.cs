// --------------------------------------------------------------------------
// <copyright file="WorkbookExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.08">Separated from NPOIExtensions</change>
// </history>

namespace ParlayRMS.ExcelService.Utility
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// Workbook Extensions.
    /// </summary>
    public static class WorkbookExtensions
    {
        /// <summary>
        /// Clones a sheet data to another sheet.
        /// </summary>
        /// <param name="workbook">Workbook instance.</param>
        /// <param name="sourceSheetName">Source sheet name.</param>
        /// <param name="newSheetName">New sheet name.</param>
        /// <returns>New Sheet instance.</returns>
        public static ISheet CloneSheet(this IWorkbook workbook, string sourceSheetName, string newSheetName)
        {
            var sheetIndex = workbook.GetSheetIndex(sourceSheetName);
            var newSheet = workbook.CloneSheet(sheetIndex);
            var newSheetIndex = workbook.GetSheetIndex(newSheet);
            workbook.SetSheetName(newSheetIndex, newSheetName);
            return newSheet;
        }
    }
}
