// --------------------------------------------------------------------------
// <copyright file="IExcelWriter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ExcelService.Core.Interfaces
{
    using System.IO;

    public interface IExcelWriter
    {
        /// <summary>
        /// Renders Excel content.
        /// </summary>
        /// <param name="model">Model that contains the data.</param>
        /// <returns>Rendered string.</returns>
        string Render(object model);

        /// <summary>
        /// Renders Excel content to Memory stream.
        /// </summary>
        /// <param name="model">Model that contains the data.</param>
        /// <returns>Memory stream.</returns>
        MemoryStream RenderMemoryStream(ExcelModel model);
    }
}