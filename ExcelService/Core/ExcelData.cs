using System;

namespace ParlayRMS.ExcelService.Core
{
    public class ExcelData
    {
        public ExcelData(string label, object value)
        {
            Label = label;
            Value = value;
        }

        public String Label { get; set; }

        public object Value { get; set; }
    }
}