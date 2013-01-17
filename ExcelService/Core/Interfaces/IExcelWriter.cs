using System.IO;

namespace ParlayRMS.ExcelService.Core.Interfaces
{
    public interface IExcelWriter
    {
        string Render(object model);

        MemoryStream RenderMemoryStream(ExcelModel model);
    }
}