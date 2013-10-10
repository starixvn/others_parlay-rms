// --------------------------------------------------------------------------
// <copyright file="ExcelRow.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ExcelService.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Excel Row class.
    /// </summary>
    public class ExcelRow
    {
        public ExcelRow()
        {
            Data = new List<ExcelData>();
        }

        public List<ExcelData> Data { get; set; }

        /// <summary>
        /// Adds new ExcelData to the list.
        /// </summary>
        /// <param name="key">The key of passed element.</param>
        /// <param name="element">Element that needs to be added to the list.</param>
        public void Add(string key, object element)
        {
            Data.Add(new ExcelData(key, element));
        }
    }
}