// --------------------------------------------------------------------------
// <copyright file="TemplateWriter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ExcelService.Writers
{
    using System.Collections.Generic;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using ParlayRMS.ExcelService.Utility;

    /// <summary>
    /// Template Writer.
    /// </summary>
    public class TemplateWriter
    {
        private ISheet _templateSheet;
        private ISheet _sheet;
        private int _rowId = 0;

        public TemplateWriter(string filePath)
            : this(new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
        }

        public TemplateWriter(FileStream fs)
            : this(new HSSFWorkbook(fs))
        {
            fs.Close();
        }

        public TemplateWriter(HSSFWorkbook template)
        {
            this._templateSheet = template.GetSheetAt(0);
            this._sheet = template.CreateSheet("Result");
        }

        public int RowId
        {
            get
            {
                return _rowId;
            }
        }

        /// <summary>
        /// Writes an Excel row.
        /// </summary>
        /// <param name="rowId">ID of row that needs to fill data into.</param>
        /// <param name="values">Values that will be filled into row.</param>
        public void WriteRow(int rowId, object[] values)
        {
            IRow row = this.CreateRow(rowId);
            row.WriteCells(0, values);
        }

        /// <summary>
        /// Writes Excel rows.
        /// </summary>
        /// <param name="rowId">ID of the first row that needs to fill data into.</param>
        /// <param name="arrayList">Data that will be filled into rows.</param>
        public void WriteRows(int rowId, List<object[]> arrayList)
        {
            int total = arrayList.Count;
            IRow row = this.CreateRows(rowId, total);
            for (int i = 0; i < total; i++)
            {
                row.WriteCells(0, arrayList[i]);
                row = this._sheet.GetRow(row.RowNum + 1);
            }
        }

        /// <summary>
        /// Writes an Excel row.
        /// </summary>
        /// <param name="rowId">ID of row that needs to fill data into.</param>
        /// <param name="o">Object that will be filled into the row.</param>
        public void WriteRow(int rowId, object o)
        {
            List<object> list = new List<object>();
            foreach (var propertyInfo in o.GetType().GetProperties())
            {
                list.Add(propertyInfo.GetValue(o, null));
            }

            WriteRow(rowId, list.ToArray());
        }

        /// <summary>
        /// Writes Excel rows.
        /// </summary>
        /// <param name="rowId">ID of the first row that needs to fill data into.</param>
        /// <param name="recordList">Data that will be filled into rows.</param>
        public void WriteRows(int rowId, IEnumerable<object> recordList)
        {
            foreach (var record in recordList)
            {
                this.WriteRow(rowId, record);
            }
        }

        /// <summary>
        /// Writes a cell range to the sheet.
        /// </summary>
        /// <param name="rowId">The index of start row.</param>
        /// <param name="totalRow">Number of rows that the cell range will span.</param>
        /// <param name="colId">The index of start column.</param>
        /// <param name="totalCol">Number of columns that the cell range will span.</param>
        /// <param name="value">Value that will br filled into the cell range.</param>
        public void WriteCellRange(int rowId, int totalRow, int colId, int totalCol, object value)
        {
            var cellRange = new NPOI.SS.Util.CellRangeAddress(rowId, rowId + totalRow - 1, colId, colId + totalCol - 1);
            this._sheet.AddMergedRegion(cellRange);
            
            var cell = this._sheet.GetCell(rowId, colId);
            cell.SetCellValue(value);
        }

        /// <summary>
        /// Resizes columns automatically.
        /// </summary>
        /// <param name="from">Start index.</param>
        /// <param name="to">End index.</param>
        public void AutosizeColumns(int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                this._sheet.AutoSizeColumn(i, false);
            }
        }

        /// <summary>
        /// Resizes a number of columns, starting from the first column of the sheet.
        /// </summary>
        /// <param name="total">Number of columns that needs to resize.</param>
        public void AutosizeColumns(int total)
        {
            this.AutosizeColumns(0, total - 1);
        }

        /// <summary>
        /// Synchronizes the columns' size with the source sheet.
        /// </summary>
        /// <param name="from">Start index.</param>
        /// <param name="to">End index.</param>
        public void SyncColumnWidths(int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                this._sheet.SetColumnWidth(i, this._templateSheet.GetColumnWidth(i));
            }
        }

        /// <summary>
        /// Synchronizes the width of a number of columns with the source sheet.
        /// </summary>
        /// <param name="total">Number of columns that needs to resize.</param>
        public void SyncColumnWidths(int total)
        {
            this.SyncColumnWidths(0, total - 1);
        }

        /// <summary>
        /// Renders the sheet.
        /// </summary>
        /// <param name="sheetName">Sheet name.</param>
        /// <returns>Memory stream that contains the sheet.</returns>
        public MemoryStream Render(string sheetName)
        {
            MemoryStream ms = new MemoryStream();
            this._sheet.Workbook.RemoveSheetAt(0);
            this._sheet.Workbook.Write(ms);
            return ms;
        }

        /// <summary>
        /// Creates a row at a specific index.
        /// </summary>
        /// <param name="rowId">Row index value.</param>
        /// <returns>A row instance.</returns>
        private IRow CreateRow(int rowId)
        {
            return CreateRows(rowId, 1);
        }

        /// <summary>
        /// Creates a number of rows, starting from a specific index.
        /// </summary>
        /// <param name="rowId">Start index.</param>
        /// <param name="total">Number of rows.</param>
        /// <returns>The first row of created rows.</returns>
        private IRow CreateRows(int rowId, int total)
        {
            this._sheet.CopyRows(this._templateSheet, rowId, this._rowId, total);
            this._rowId += total;
            return this._sheet.GetRow(this._rowId - total);
        }
    }
}