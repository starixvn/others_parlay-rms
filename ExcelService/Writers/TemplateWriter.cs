namespace ExcelService.Writers
{
    using System.Collections.Generic;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Utility;

    public class TemplateWriter
    {
        private readonly ISheet _templateSheet;
        private readonly ISheet _sheet;

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
            _templateSheet = template.GetSheetAt(0);
            _sheet = template.CreateSheet("Result");
        }

        public int RowId { get; private set; }

        public void WriteCell(int rowId, int columnId, object value)
        {
            var cell = _sheet.GetCell(rowId, columnId);
            cell.SetCellValue(value);
        }

        public void WriteRow(int rowId, object[] values)
        {
            var row = CreateRow(rowId);
            row.WriteCells(0, values);
        }

        public void WriteRows(int rowId, List<object[]> arrayList)
        {
            var total = arrayList.Count;
            var row = CreateRows(rowId, total);
            for (var i = 0; i < total; i++)
            {
                row.WriteCells(0, arrayList[i]);
                row = _sheet.GetRow(row.RowNum + 1);
            }
        }

        public void WriteRow(int rowId, object o)
        {
            var list = new List<object>();
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
                WriteRow(rowId, record);
            }
        }

        public void WriteCellRange(int rowId, int totalRow, int colId, int totalCol, object value)
        {
            var cellRange = new NPOI.SS.Util.CellRangeAddress(rowId, rowId + totalRow - 1, colId, colId + totalCol - 1);
            _sheet.AddMergedRegion(cellRange);

            var cell = _sheet.GetCell(rowId, colId);
            cell.SetCellValue(value);
        }

        public void AutosizeColumns(int from, int to)
        {
            for (var i = from; i <= to; i++)
            {
                _sheet.AutoSizeColumn(i, false);
            }
        }

        public void AutosizeColumns(int total)
        {
            AutosizeColumns(0, total - 1);
        }

        public void SyncColumnWidths(int from, int to)
        {
            for (var i = from; i <= to; i++)
            {
                _sheet.SetColumnWidth(i, _templateSheet.GetColumnWidth(i));
            }
        }

        public void SyncColumnWidths(int total)
        {
            SyncColumnWidths(0, total - 1);
        }

        public MemoryStream Render(string sheetName)
        {
            var ms = new MemoryStream();
            _sheet.Workbook.RemoveSheetAt(0);
            _sheet.Workbook.Write(ms);
            return ms;
        }

        private IRow CreateRow(int rowId)
        {
            return CreateRows(rowId, 1);
        }

        private IRow CreateRows(int rowId, int total)
        {
            _sheet.CopyRows(_templateSheet, rowId, RowId, total);
            RowId += total;
            return _sheet.GetRow(RowId - total);
        }
    }
}
