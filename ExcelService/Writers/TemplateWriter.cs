// -----------------------------------------------------------------------
// <copyright file="TemplateWriter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ExcelService.Writers
{
    using System.Collections.Generic;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using ParlayRMS.ExcelService.Utility;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TemplateWriter
    {
        private ISheet templateSheet;
        private ISheet sheet;
        private int rowId = 0;

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
            this.templateSheet = template.GetSheetAt(0);
            this.sheet = template.CreateSheet("Result");
        }

        private IRow CreateRow(int rowId)
        {
            return CreateRows(rowId, 1);
        }

        private IRow CreateRows(int rowId, int total)
        {
            this.sheet.CopyRows(this.templateSheet, rowId, this.rowId, total);
            this.rowId += total;
            return this.sheet.GetRow(this.rowId - total);
        }

        public void WriteRow(int rowId, object[] values)
        {
            IRow row = this.CreateRow(rowId);
            row.WriteCells(0, values);
        }

        public void WriteRows(int rowId, List<object[]> arrayList)
        {
            int total = arrayList.Count;
            IRow row = this.CreateRows(rowId, total);
            for (int i = 0; i < total; i++)
            {
                row.WriteCells(0, arrayList[i]);
                row = this.sheet.GetRow(row.RowNum + 1);
            }
        }

        public void WriteRow(int rowId, object o)
        {
            List<object> list = new List<object>();
            foreach (var propertyInfo in o.GetType().GetProperties())
            {
                list.Add(propertyInfo.GetValue(o, null));
            }

            WriteRow(rowId, list.ToArray());
        }

        public void WriteRows(int rowId, IEnumerable<object> recordList)
        {
            foreach (var record in recordList)
            {
                this.WriteRow(rowId, record);
            }
        }

        public void AutosizeColumns(int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                this.sheet.AutoSizeColumn(i, false);
            }
        }

        public void AutosizeColumns(int total)
        {
            this.AutosizeColumns(0, total - 1);
        }

        public void SyncColumnWidths(int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                this.sheet.SetColumnWidth(i, this.templateSheet.GetColumnWidth(i));
            }
        }

        public void SyncColumnWidths(int total)
        {
            this.SyncColumnWidths(0, total - 1);
        }

        public MemoryStream Render(string sheetName)
        {
            MemoryStream ms = new MemoryStream();
            this.sheet.Workbook.RemoveSheetAt(0);
            this.sheet.Workbook.Write(ms);
            return ms;
        }
    }
}