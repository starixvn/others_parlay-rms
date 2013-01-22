namespace ParlayRMS.Services.Interface.Members
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.Models;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO.Members;

    public interface IMembersService
    {
        IEnumerable<RMS_CustomerSel> GetListCustomer(string username, int? level);

        Result AddCustomer(string userName, int level, int adminId, string ip);

        Result UpdateLevelCustomer(int custId, int level, int adminId, string ip);

        Result UpdateStatusCustomer(int custId, bool following, int adminId, string ip);

        bool CheckExistingCustomer(int custId);

        Result ChangeLevelCustomer(int custId, string userName, int level, int adminId, string ip);
    }
}