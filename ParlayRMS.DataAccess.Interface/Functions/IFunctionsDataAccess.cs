namespace ParlayRMS.DataAccess.Interface.Functions
{
    using System.Collections.Generic;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO.Functions;
    using ParlayRMS.DTO;

    public interface IFunctionsDataAccess
    {
        Result RMS_FunctionIns(string name, string key, string description, int parentId, string parentName, int displayOrder, int adminId, string ip, out int functionId);

        Result RMS_FunctionUpd(int id, string name, string description, int parentId, string parentName, int displayOrder, int adminId, string ip);

        IEnumerable<RMS_FunctionSel> RMS_FunctionSel();
    }
}