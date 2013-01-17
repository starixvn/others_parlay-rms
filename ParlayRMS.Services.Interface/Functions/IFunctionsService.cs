namespace ParlayRMS.Services.Interface.Functions
{
    using System.Collections.Generic;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO;

    public interface IFunctionsService
    {
        Result CreateFunction(string name, string key, string description, int parentId, string parentName, int displayOrder, int adminId, string ip, out int functionId);

        Result UpdateFunction(int id, string name, string description, int parentId, string parentName, int displayOrder, int adminId, string ip);

        IEnumerable<Function> GetAllFunctions();

        IEnumerable<Function> GetAllFunctionsInOrder();

        IEnumerable<Function> GetRootFunctions();
    }
}