namespace ExcelService.Utility
{
    using NPOI.SS.UserModel;

    public static class RowExtensions
    {
        public static ICell CreateCell(this IRow row, int colId, ICellStyle cellStyle)
        {
            ICell cell = row.CreateCell(colId);
            cell.CellStyle = cellStyle;
            return cell;
        }

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