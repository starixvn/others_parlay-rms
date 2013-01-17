using System.Collections.Generic;

namespace ParlayRMS.ExcelService.Core
{
    public class ExcelRow
    {
        public ExcelRow()
        {
            Data = new List<ExcelData>();
        }

        public List<ExcelData> Data { get; set; }

        public void Add(string key, object element)
        {
            Data.Add(new ExcelData(key, element));
        }
    }
}