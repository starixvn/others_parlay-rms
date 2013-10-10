// --------------------------------------------------------------------------
// <copyright file="ExcelResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ExcelService.Web.Mvc
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using ParlayRMS.ExcelService.Core;
    using ParlayRMS.ExcelService.Writers;

    /// <summary>
    /// Excel Result.
    /// </summary>
    public class ExcelResult : ActionResult
    {
        public ExcelResult(object model)
            : this(model, null)
        {
        }

        public ExcelResult(object model, string fileName)
        {
            FileName = fileName ?? string.Format("newfile-{0}.xls", DateTime.Now.Ticks);
            Model = model;
        }

        public string FileName { get; set; }

        public object Model { get; set; }

        /// <summary>
        /// Executes Result.
        /// </summary>
        /// <param name="context">Controller Context.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (!string.IsNullOrEmpty(FileName))
            {
                context.HttpContext.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                context.HttpContext.Response.AddHeader("content-type", "application/vnd.ms-excel");
            }

            var svc = new ExcelDocument(new ExcelNpoiWriter());
            MemoryStream ms = svc.RenderMemoryStream(Model);
            context.HttpContext.Response.BinaryWrite(ms.ToArray());
        }
    }
}