// --------------------------------------------------------------------------
// <copyright file="RowExtensions.cs" company="Nexcel Solutions Vietnam">
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
    /// Row Extensions.
    /// </summary>
    public static class RowExtensions
    {
        /// <summary>
        /// Creates new cell.
        /// </summary>
        /// <param name="row">Row instance.</param>
        /// <param name="colId">Index of column where the cell will be created.</param>
        /// <param name="cellStyle">Cell Style that will apply to the created cell.</param>
        /// <returns>New cell instance.</returns>
        public static ICell CreateCell(this IRow row, int colId, ICellStyle cellStyle)
        {
            ICell cell = row.CreateCell(colId);
            cell.CellStyle = cellStyle;
            return cell;
        }

        /// <summary>
        /// Writes values for a number of cells on a same row.
        /// </summary>
        /// <param name="row">Row instance where the values will be written.</param>
        /// <param name="colId">Start column index.</param>
        /// <param name="values">Values that will be written to the row.</param>
        public static void WriteCells(this IRow row, int colId, object[] values)
        {
            int id = colId;
            foreach (var value in values)
            {
                row.GetCell(id).SetCellValue(value);
                id++;
            }
        }
    }
}
