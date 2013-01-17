using System.Collections.Generic;

namespace ParlayRMS.ExcelService.Core
{
    public class ExcelModel
    {
        public ExcelModel()
        {
            Data = new List<ExcelRow>();
        }

        public List<ExcelRow> Data { get; set; }

        public void Add(ExcelRow row)
        {
            Data.Add(row);
        }
    }
}