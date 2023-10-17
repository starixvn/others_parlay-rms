namespace ExcelService.Utility
{
    using System;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    public static class CellExtensions
    {
        public static void SetCellValue(this ICell cell, object value)
        {
            if (value == null)
            {
                cell.SetCellValue("0");
            }
            else
            {
                switch (value)
                {
                    case string s:
                        cell.SetCellValue(s);
                        break;
                    case int i:
                        cell.SetCellValue(i);
                        break;
                    case long l:
                        cell.SetCellValue(l);
                        break;
                    case double d:
                        cell.SetCellValue(d);
                        break;
                    case decimal value1:
                        cell.SetCellValue(value1);
                        break;
                    case bool b:
                        cell.SetCellValue(b);
                        break;
                    case DateTime time:
                        cell.SetCellValue(time);
                        break;
                    case IRichTextString textString:
                        cell.SetCellValue(textString);
                        break;
                    default:
                        cell.SetCellValue("Undefined");
                        break;
                }
            }
        }

        public static void SetCellValue(this ICell cell, double? value)
        {
            value = value ?? 0;
            cell.SetCellValue((double)value);
        }

        public static void SetCellValue(this ICell cell, decimal value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        public static void SetCellValue(this ICell cell, decimal? value)
        {
            value = value ?? 0;
            cell.SetCellValue((decimal)value);
        }

        public static void SetCellValue(this ICell cell, int value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        public static void SetCellValue(this ICell cell, int? value)
        {
            value = value ?? 0;
            cell.SetCellValue((int)value);
        }

        public static void SetCellValue(this ICell cell, long value)
        {
            cell.SetCellValue(Convert.ToDouble(value));
        }

        public static void SetCellValue(this ICell cell, long? value)
        {
            value = value ?? 0;
            cell.SetCellValue((long)value);
        }

        public static void CopyFrom(this ICell cell, ICell sourceCell)
        {
            CellRangeAddress sourceRange = null;
            for (var i = 0; i < sourceCell.Sheet.NumMergedRegions; i++)
            {
                var range = sourceCell.Sheet.GetMergedRegion(i);
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

            cell.CellStyle = sourceCell.CellStyle;
        }
    }
}
