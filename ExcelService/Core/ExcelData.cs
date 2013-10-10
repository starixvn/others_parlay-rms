// --------------------------------------------------------------------------
// <copyright file="ExcelData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ExcelService.Core
{
    using System;

    /// <summary>
    /// Excel Data model.
    /// </summary>
    public class ExcelData
    {
        public ExcelData(string label, object value)
        {
            Label = label;
            Value = value;
        }

        public string Label { get; set; }

        public object Value { get; set; }
    }
}