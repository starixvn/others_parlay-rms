using System;
using System.IO;
using System.Web.Mvc;
using ParlayRMS.ExcelService.Core;
using ParlayRMS.ExcelService.Writers;

namespace ParlayRMS.ExcelService.Web.Mvc
{
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

        public String FileName { get; set; }

        public object Model { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (!String.IsNullOrEmpty(FileName))
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