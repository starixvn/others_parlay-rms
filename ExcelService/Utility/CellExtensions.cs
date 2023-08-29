// --------------------------------------------------------------------------
// <copyright file="CellExtensions.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.08">Separated from NPOIExtensions</change>
// </history>

namespace ExcelService.Utility
{
    using System;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Cell Extensions.
    /// </summary>
    public static class CellExtensions
    {
        /// <summary>
        /// Sets a value to a cell.
        /// </summary>
        /// <param name="cell">Cell instance that will take the value.</param>
        /// <param name="value">Value that will be filled into the cell.</param>
        public static void SetCellValue(this ICell cell, object value)
        {
            if (value == null)
            {
                cell.SetCellValue("0");
            }
            else
            {
                if (value is string)
                {
                    cell.SetCellValue((string)value);
                }
                else if (value is int)
                {
                    cell.SetCellValue((int)value);
                }
                else if (value is long)
                {
                    cell.SetCellValue((long)value);
                }
                else if (value is double)
                {
                    cell.SetCellValue((double)value);
                }
                else if (value is decimal)
                {
                    cell.SetCellValue((decimal)value);
                }
                else if (value is bool)
                {
                    cell.SetCellValue((bool)value);
                }
                else if (value is System.DateTime)
                {
                    cell.SetCellValue((System.DateTime)value);
                }
                else if (value is IRichTextString)
                {
                    cell.SetCellValue((IRichTextString)value);
                }
                else
                {
                    cell.SetCellValue("Undefined");
                }
            }
        }

        /// <summary>
        /// Sets a value to a cell.
        /// </summary>
        /// <param name="cell">Cell instance that will take the value.</param>
        /// <param name="value">Value that will be filled into the cell.</param>
        public static void SetCellValue(this ICell cell, double? value)
        {
            value = value ?? 0;
            cell.SetCellValue((double)value);
        }

        /// <summary>
        /// Sets a value to a cell.
        /// </summary>
        /// <param name="cell">Cell instance that will take the value.</param>
        /// <param name="value">Value that will be filled into the cell.</param>
        public static void SetCellValue(this ICell cell, decimal value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        /// <summary>
        /// Sets a value to a cell.
        /// </summary>
        /// <param name="cell">Cell instance that will take the value.</param>
        /// <param name="value">Value that will be filled into the cell.</param>
        public static void SetCellValue(this ICell cell, decimal? value)
        {
            value = value ?? 0;
            cell.SetCellValue((decimal)value);
        }

        /// <summary>
        /// Sets a value to a cell.
        /// </summary>
        /// <param name="cell">Cell instance that will take the value.</param>
        /// <param name="value">Value that will be filled into the cell.</param>
        public static void SetCellValue(this ICell cell, int value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        /// <summary>
        /// Sets a value to a cell.
        /// </summary>
        /// <param name="cell">Cell instance that will take the value.</param>
        /// <param name="value">Value that will be filled into the cell.</param>
        public static void SetCellValue(this ICell cell, int? value)
        {
            value = value ?? 0;
            cell.SetCellValue((int)value);
        }

        /// <summary>
        /// Sets a value to a cell.
        /// </summary>
        /// <param name="cell">Cell instance that will take the value.</param>
        /// <param name="value">Value that will be filled into the cell.</param>
        public static void SetCellValue(this ICell cell, long value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        /// <summary>
        /// Sets a value to a cell.
        /// </summary>
        /// <param name="cell">Cell instance that will take the value.</param>
        /// <param name="value">Value that will be filled into the cell.</param>
        public static void SetCellValue(this ICell cell, long? value)
        {
            value = value ?? 0;
            cell.SetCellValue((long)value);
        }

        /// <summary>
        /// Copies value from a cell to another cell.
        /// </summary>
        /// <param name="cell">Cell instance that will take the value.</param>
        /// <param name="sourceCell">Cell instance that will give the value.</param>
        public static void CopyFrom(this ICell cell, ICell sourceCell)
        {
            // Copy Merged
            CellRangeAddress sourceRange = null;
            for (var i = 0; i < sourceCell.Sheet.NumMergedRegions; i++)
            {
                CellRangeAddress range = sourceCell.Sheet.GetMergedRegion(i);
                if (range.FirstColumn <= sourceCell.ColumnIndex
                    && range.LastColumn >= sourceCell.ColumnIndex
                    && range.FirstRow <= sourceCell.RowIndex
                    && range.LastRow >= sourceCell.RowIndex)
                {
                    sourceRange = range;
                    break;
                }
            }

            if (sourceRange != null)
            {
                cell.Sheet.AddMergedRegion(sourceRange.Clone());
            }

            // copy Style
            cell.CellStyle = sourceCell.CellStyle;
        }
    }
}