// --------------------------------------------------------------------------
// <copyright file="HSSFSheetExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.08">Separated from NPOIExtensions</change>
// </history>

namespace ParlayRMS.ExcelService.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Sheet Extensions.
    /// </summary>
    public static class HSSFSheetExtensions
    {
        /// <summary>
        /// Copies a number of rows from a sheet to another sheet.
        /// </summary>
        /// <param name="sheet">Destination sheet.</param>
        /// <param name="sourceSheet">Source sheet.</param>
        /// <param name="sourceRowNum">Start row index in the source sheet.</param>
        /// <param name="destinateRowNum">Start row index in the destination sheet.</param>
        /// <param name="total">Total of rows that will be copied.</param>
        public static void CopyRows(this ISheet sheet, ISheet sourceSheet, int sourceRowNum, int destinateRowNum, int total)
        {
            if (total > 0)
            {
                for (var i = 0; i < total; i++)
                {
                    sheet.CopyRow(sourceSheet, sourceRowNum + i, destinateRowNum + i);
                }
            }
        }

        /// <summary>
        /// Copies a row from a sheet to another sheet.
        /// </summary>
        /// <param name="sheet">Destination sheet.</param>
        /// <param name="sourceSheet">Source sheet.</param>
        /// <param name="sourceRowNum">Row index in the source sheet.</param>
        /// <param name="destinateRowNum">Row index in the destination sheet.</param>
        /// <param name="firstColumn">Index of copying area's start column.</param>
        /// <param name="lastColumn">Index of copying area's end column.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public static void CopyRow(this ISheet sheet, ISheet sourceSheet, int sourceRowNum, int destinateRowNum, int firstColumn, int lastColumn)
        {
            if (sheet.LastRowNum < destinateRowNum)
            {
                for (var i = sheet.LastRowNum + 1; i <= destinateRowNum; i++)
                {
                    sheet.CreateRow(i);
                }
            }

            // Get the source / new row
            var sourceRow = sourceSheet.GetRow(sourceRowNum);
            if (sourceRow == null)
            {
                return;
            }

            var newRow = sheet.GetRow(destinateRowNum);
            if (newRow == null)
            {
                newRow = sheet.CreateRow(destinateRowNum);
            }

            newRow.Height = sourceRow.Height;

            // Loop through source columns to add to new row
            int firstCellNum = Math.Min(sourceRow.FirstCellNum, firstColumn);
            int lastCellNum = Math.Min(sourceRow.LastCellNum, lastColumn + 1);
            for (int i = firstCellNum; i < lastCellNum; i++)
            {
                // Grab a copy of the old/new cell
                var oldCell = sourceRow.GetCell(i);

                // If the old cell is null jump to next cell
                if (oldCell == null)
                {
                    continue;
                }

                var newCell = newRow.CreateCell(i);
                CloneCell(newCell, oldCell);

                // copy Merged
                CellRangeAddress sourceRange = null;
                for (var j = 0; j < sourceSheet.NumMergedRegions; j++)
                {
                    CellRangeAddress range = sourceSheet.GetMergedRegion(j);
                    if (range.FirstColumn == oldCell.ColumnIndex
                        && range.FirstRow == oldCell.RowIndex)
                    {
                        sourceRange = range;
                        break;
                    }
                }

                if (sourceRange != null)
                {
                    CellRangeAddress newRange = new CellRangeAddress(
                                                        newCell.RowIndex,
                                                        newCell.RowIndex + sourceRange.LastRow - sourceRange.FirstRow,
                                                        newCell.ColumnIndex,
                                                        newCell.ColumnIndex + sourceRange.LastColumn - sourceRange.FirstColumn);
                    sheet.AddMergedRegion(newRange);
                }
            }
        }

        /// <summary>
        /// Copies a row from a sheet to another sheet.
        /// </summary>
        /// <param name="sheet">Destination sheet.</param>
        /// <param name="sourceSheet">Source sheet.</param>
        /// <param name="sourceRowNum">Row index in the source sheet.</param>
        /// <param name="destinateRowNum">Row index in the destination sheet.</param>
        public static void CopyRow(this ISheet sheet, ISheet sourceSheet, int sourceRowNum, int destinateRowNum)
        {
            sheet.CopyRow(sourceSheet, sourceRowNum, destinateRowNum, 0, int.MaxValue - 1);
        }

        /// <summary>
        /// Removes a row from sheet.
        /// </summary>
        /// <param name="sheet">Sheet instance.</param>
        /// <param name="rowIndex">Index of removing row.</param>
        public static void RemoveRow(this ISheet sheet, int rowIndex)
        {
            sheet.RemoveRow(sheet.GetRow(rowIndex));
        }

        /// <summary>
        /// Removes a number of columns from the sheet.
        /// </summary>
        /// <param name="sheet">Sheet instance.</param>
        /// <param name="columns">List of removing columns.</param>
        public static void RemoveColumns(this ISheet sheet, List<int> columns)
        {
            columns.Sort();
            for (int i = columns.Count - 1; i >= 0; i--)
            {
                RemoveColumn(sheet, columns[i]);
            }
        }

        /// <summary>
        /// Removes a column from the sheet.
        /// </summary>
        /// <param name="sheet">Sheet instance.</param>
        /// <param name="columnIndex">Index of removing column.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public static void RemoveColumn(this ISheet sheet, int columnIndex)
        {
            int maxColumn = 0;
            bool adjustColumn = true;
            for (int r = sheet.FirstRowNum; r <= sheet.LastRowNum; r++)
            {
                var row = sheet.GetRow(r);

                // if no row exists here; then nothing to do; next!
                if (row == null)
                {
                    continue;
                }

                // if the row doesn't have this many columns then we are good; next!
                int lastColumn = row.LastCellNum;
                maxColumn = Math.Max(maxColumn, lastColumn);

                if (lastColumn < columnIndex)
                {
                    continue;
                }

                for (int x = columnIndex; x < lastColumn; x++)
                {
                    var oldCell = row.GetCell(x);
                    if (oldCell != null)
                    {
                        row.RemoveCell(oldCell);
                    }

                    if (x < lastColumn)
                    {
                        // move cell
                        var nextCell = row.GetCell(x + 1);
                        if (nextCell != null)
                        {
                            var newCell = row.CreateCell(x, nextCell.CellType);
                            if (adjustColumn)
                            {
                                sheet.SetColumnWidth(newCell.ColumnIndex, sheet.GetColumnWidth(nextCell.ColumnIndex));
                            }

                            CloneCell(newCell, nextCell);
                        }
                    }
                    else
                    {
                        var removeCell = row.GetCell(x);
                        if (removeCell != null)
                        {
                            row.RemoveCell(removeCell);
                        }
                    }
                }

                // Adjust merged region
                for (int i = sheet.NumMergedRegions - 1; i >= 0; i--)
                {
                    var mergedRegion = sheet.GetMergedRegion(i);

                    if (r == mergedRegion.FirstRow)
                    {
                        if (columnIndex == mergedRegion.FirstColumn && columnIndex == mergedRegion.LastColumn)
                        {
                            sheet.RemoveMergedRegion(i);
                        }
                        else if (mergedRegion.FirstColumn > columnIndex)
                        {
                            mergedRegion.FirstColumn = mergedRegion.FirstColumn - 1;
                            mergedRegion.LastColumn = mergedRegion.LastColumn - 1;
                        }
                        else if (columnIndex <= mergedRegion.LastColumn)
                        {
                            mergedRegion.LastColumn = mergedRegion.LastColumn - 1;
                        }
                    }
                }

                adjustColumn = false;
            }
        }

        /// <summary>
        /// Clones a cell value and style to another cell.
        /// </summary>
        /// <param name="sheet">Sheet instance.</param>
        /// <param name="newCell">New cell instance.</param>
        /// <param name="oldCell">Old cell instance.</param>
        public static void CloneCell(this ISheet sheet, ICell newCell, ICell oldCell)
        {
            CloneCell(newCell, oldCell);
        }

        /// <summary>
        /// Gets a cell by row and column indexes.
        /// </summary>
        /// <param name="sheet">Sheet instance.</param>
        /// <param name="rowIndex">Row index.</param>
        /// <param name="colIndex">Column index.</param>
        /// <returns>Cell instance.</returns>
        public static ICell GetCell(this ISheet sheet, int rowIndex, int colIndex)
        {
            return sheet.GetRow(rowIndex).GetCell(colIndex);
        }

        /// <summary>
        /// Merges cells into a region.
        /// </summary>
        /// <param name="sheet">Sheet instance.</param>
        /// <param name="firstRow">First row of merged region.</param>
        /// <param name="firstCol">First column of merged region.</param>
        /// <param name="lastRow">Last row of merged region.</param>
        /// <param name="lastCol">Last column of merged region.</param>
        /// <returns>Cell Range instance index.</returns>
        public static int MergeCells(this ISheet sheet, int firstRow, int firstCol, int lastRow, int lastCol)
        {
            var rangeAddress = new CellRangeAddress(firstRow, lastRow, firstCol, lastCol);
            return sheet.AddMergedRegion(rangeAddress);
        }

        /// <summary>
        /// Removes a region from sheet.
        /// </summary>
        /// <param name="sheet">Sheet instance.</param>
        /// <param name="firstRow">First row of merged region.</param>
        /// <param name="firstCol">First column of merged region.</param>
        /// <param name="lastRow">Last row of merged region.</param>
        /// <param name="lastCol">Last column of merged region.</param>
        public static void RemoveMergedRegions(this ISheet sheet, int firstRow, int firstCol, int lastRow, int lastCol)
        {
            for (int i = sheet.NumMergedRegions - 1; i >= 0; i--)
            {
                var mergedRegion = sheet.GetMergedRegion(i);
                if (mergedRegion.FirstRow >= firstRow && mergedRegion.LastRow <= lastRow &&
                    mergedRegion.FirstColumn >= firstCol && mergedRegion.LastColumn <= lastCol)
                {
                    sheet.RemoveMergedRegion(i);
                }
            }
        }

        /// <summary>
        /// Creates a cell at specific row and column.
        /// </summary>
        /// <param name="sheet">Sheet instance.</param>
        /// <param name="rowId">Row index.</param>
        /// <param name="colId">Column index.</param>
        /// <param name="rowSpan">Number of rows it will span.</param>
        /// <param name="colSpan">Number of columns it will span.</param>
        /// <returns>Cell instance.</returns>
        public static ICell CreateCell(this ISheet sheet, int rowId, int colId, int rowSpan, int colSpan)
        {
            Debug.Assert(rowSpan >= 0, "rowSpan must be more than or equal 0");
            Debug.Assert(colSpan >= 0, "colSpan must be more than or equal 0");

            ICell cell = sheet.CreateCell(rowId, colId);
            if (rowSpan == 0 && colSpan == 0)
            {
                return cell;
            }

            MergeCells(sheet, rowId, colId, rowId + rowSpan, colId + colSpan);
            return cell;
        }

        /// <summary>
        /// Creates a cell at specific row and column.
        /// </summary>
        /// <param name="sheet">Sheet instance.</param>
        /// <param name="rowId">Row index.</param>
        /// <param name="colId">Column index.</param>
        /// <param name="rowSpan">Number of rows it will span.</param>
        /// <param name="colSpan">Number of columns it will span.</param>
        /// <param name="cellStyle">Cell Style that will apply to the created cell.</param>
        /// <returns>Cell instance.</returns>
        public static ICell CreateCell(this ISheet sheet, int rowId, int colId, int rowSpan, int colSpan, ICellStyle cellStyle)
        {
            ICell cell = sheet.CreateCell(rowId, colId, rowSpan, colSpan);
            cell.CellStyle = cellStyle;
            return cell;
        }

        /// <summary>
        /// Creates a cell at specific row and column.
        /// </summary>
        /// <param name="sheet">Sheet instance.</param>
        /// <param name="rowId">Row index.</param>
        /// <param name="colId">Column index.</param>
        /// <returns>Cell instance.</returns>
        public static ICell CreateCell(this ISheet sheet, int rowId, int colId)
        {
            IRow row = sheet.GetRow(rowId);
            if (row == null)
            {
                row = sheet.CreateRow(rowId);
            }

            ICell cell = row.GetCell(colId);
            if (cell == null)
            {
                cell = row.CreateCell(colId);
            }

            return cell;
        }

        /// <summary>
        /// Clones a cell's value and style to another cell.
        /// </summary>
        /// <param name="newCell">New cell instance.</param>
        /// <param name="oldCell">Old cell instance.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        private static void CloneCell(ICell newCell, ICell oldCell)
        {
            if (oldCell.CellComment != null)
            {
                newCell.CellComment = oldCell.CellComment;
            }

            if (oldCell.Hyperlink != null)
            {
                newCell.Hyperlink = oldCell.Hyperlink;
            }

            if (oldCell.CellStyle != null)
            {
                newCell.CellStyle = oldCell.CellStyle;
            }

            newCell.SetCellType(oldCell.CellType);

            switch (newCell.CellType)
            {
                case CellType.BOOLEAN:
                    {
                        newCell.SetCellValue(oldCell.BooleanCellValue);
                        break;
                    }

                case CellType.NUMERIC:
                    {
                        newCell.SetCellValue(oldCell.NumericCellValue);
                        break;
                    }

                case CellType.STRING:
                    {
                        newCell.SetCellValue(oldCell.StringCellValue);
                        break;
                    }

                case CellType.ERROR:
                    {
                        newCell.SetCellValue(oldCell.ErrorCellValue);
                        break;
                    }

                case CellType.FORMULA:
                    {
                        newCell.CellFormula = oldCell.CellFormula;
                        break;
                    }
            }
        }
    }
}
