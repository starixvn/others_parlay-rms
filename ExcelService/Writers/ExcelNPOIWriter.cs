// --------------------------------------------------------------------------
// <copyright file="ExcelNpoiWriter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ExcelService.Writers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using ParlayRMS.ExcelService.Core;
    using ParlayRMS.ExcelService.Core.Interfaces;

    /// <summary>
    /// Excel NPOI Writer.
    /// </summary>
    public class ExcelNpoiWriter : IExcelWriter
    {
        #region IExcelWriter Members

        /// <summary>
        /// Renders Excel content.
        /// </summary>
        /// <param name="model">Model that contains the data.</param>
        /// <returns>Rendered string.</returns>
        public string Render(object model)
        {
            string returnString = string.Empty;
            if (model is IEnumerable)
            {
                foreach (object item in (IEnumerable)model)
                {
                    Type type = item.GetType();
                    PropertyInfo[] properties = item.GetType().GetProperties();
                    foreach (PropertyInfo info in properties)
                    {
                        PropertyInfo valuePropetry = type.GetProperty(info.Name);
                        object value = valuePropetry.GetValue(item, null);
                        returnString += string.Format("{0} : {1}", info.Name, value) + "<BR>";
                    }
                }
            }

            return returnString;
        }

        /// <summary>
        /// Renders Excel content to Memory stream.
        /// </summary>
        /// <param name="model">Model that contains the data.</param>
        /// <returns>Memory stream.</returns>
        public MemoryStream RenderMemoryStream(ExcelModel model)
        {
            var wb = new HSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Data");
            ICellStyle styleHeader = GetHeaderStyle(wb);
            ICellStyle style = GetNormalStyle(wb);
            List<ExcelRow> excelData = model.Data;
            int counter = 0;
            foreach (ExcelRow excelRow in excelData)
            {
                IRow row = sheet.CreateRow(counter);
                int propertyCounter = 0;
                if (counter == 0)
                {
                    foreach (ExcelData element in excelRow.Data)
                    {
                        CreateCell(styleHeader, row, propertyCounter, element.Label);
                        propertyCounter++;
                    }

                    counter++;
                    row = sheet.CreateRow(counter);
                }

                propertyCounter = 0;
                foreach (ExcelData element in excelRow.Data)
                {
                    object value = element.Value;
                    CreateCell(style, row, propertyCounter, value != null ? value.ToString() : string.Empty);
                    propertyCounter++;
                }

                counter++;
            }

            var ms = new MemoryStream();
            wb.Write(ms);
            return ms;
        }

        #endregion IExcelWriter Members

        /// <summary>
        /// Gets default header style.
        /// </summary>
        /// <param name="wb">Workbook instance.</param>
        /// <returns>Default header style.</returns>
        private static ICellStyle GetHeaderStyle(HSSFWorkbook wb)
        {
            ICellStyle style = wb.CreateCellStyle();
            style.FillForegroundColor = HSSFColor.LIGHT_BLUE.index;
            style.FillPattern = FillPatternType.SOLID_FOREGROUND;

            IFont font = wb.CreateFont();
            font.Color = HSSFColor.WHITE.index;
            font.Boldweight = (short)FontBoldWeight.BOLD;
            style.SetFont(font);

            return style;
        }

        /// <summary>
        /// Gets normal style of a cell.
        /// </summary>
        /// <param name="wb">Workbook instance.</param>
        /// <returns>Normal style of a cell.</returns>
        private static ICellStyle GetNormalStyle(HSSFWorkbook wb)
        {
            ICellStyle style = wb.CreateCellStyle();
            style.FillForegroundColor = HSSFColor.WHITE.index;
            style.FillPattern = FillPatternType.SOLID_FOREGROUND;
            style.BorderBottom = style.BorderLeft = style.BorderRight = style.BorderTop = BorderStyle.THIN;

            IFont font = wb.CreateFont();
            font.Color = HSSFColor.BLACK.index;
            font.Boldweight = (short)FontBoldWeight.BOLD;
            style.SetFont(font);

            return style;
        }

        /// <summary>
        /// Creates a cell at a specific column of the row and set a given value to it.
        /// </summary>
        /// <param name="style">Cell style that will apply to the cell.</param>
        /// <param name="row">Row instance.</param>
        /// <param name="index">Index of column.</param>
        /// <param name="value">Value that will be set to the cell.</param>
        private static void CreateCell(ICellStyle style, IRow row, int index, string value)
        {
            var cell = row.CreateCell(index);
            cell.SetCellValue(value);
            cell.CellStyle = style;
        }
    }
}