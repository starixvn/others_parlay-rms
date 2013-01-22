namespace ParlayRMS.DataAccess.Interface.Members
{
    using System.Collections.Generic;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO.Members;
    using ParlayRMS.DTO;

    public interface IMembersDataAccess
    {
        IEnumerable<RMS_CustomerSel> RMS_CustomerSel(string username, int? level);

        Result RMS_CustomerIns(string userName, int level, int adminId, string ip);

        Result RMS_CustomerUpd(int custId, int level, int adminId, string ip);

        Result RMS_CustomerUpdStatus(int custId, bool following, int adminId, string ip);

        bool RMS_CustomerCheckExisting(int custId);
    }
}