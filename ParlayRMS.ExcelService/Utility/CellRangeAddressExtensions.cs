namespace ExcelService.Utility
{
    using NPOI.SS.Util;

    public static class CellRangeAddressExtensions
    {
        public static CellRangeAddress Clone(this CellRangeAddress range)
        {
            var result = new CellRangeAddress(range.FirstRow, range.LastRow, range.FirstColumn, range.LastColumn);
            return result;
        }
    }
}
