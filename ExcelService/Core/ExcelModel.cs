// --------------------------------------------------------------------------
// <copyright file="ExcelModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ExcelService.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Excel Model.
    /// </summary>
    public class ExcelModel
    {
        public ExcelModel()
        {
            Data = new List<ExcelRow>();
        }

        public List<ExcelRow> Data { get; set; }

        /// <summary>
        /// Adds a row to the ExcelModel instance.
        /// </summary>
        /// <param name="row">Row that needs to be added to the ExcelModel instance.</param>
        public void Add(ExcelRow row)
        {
            Data.Add(row);
        }
    }
}