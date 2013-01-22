namespace ParlayRMS.Services.Implement.Members
{
    using System.Collections.Generic;
    using ParlayRMS.DataAccess.Implement.Members;
    using ParlayRMS.DataAccess.Interface.Members;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Members;
    using ParlayRMS.Services.Interface.Members;

    public class MembersService : BaseService, IMembersService
    {
        private readonly IMembersDataAccess customerDataAccess = new MembersDataAccess();

        public IEnumerable<RMS_CustomerSel> GetListCustomer(string username, int? level)
        {
            return this.customerDataAccess.RMS_CustomerSel(username, level);
        }

        public Result AddCustomer(string userName, int level, int adminId, string ip)
        {
            return this.customerDataAccess.RMS_CustomerIns(userName, level, adminId, ip);
        }

        public Result UpdateLevelCustomer(int custId, int level, int adminId, string ip)
        {
            return this.customerDataAccess.RMS_CustomerUpd(custId, level, adminId, ip);
        }

        public Result UpdateStatusCustomer(int custId, bool following, int adminId, string ip)
        {
            return this.customerDataAccess.RMS_CustomerUpdStatus(custId, following, adminId, ip);
        }

        public bool CheckExistingCustomer(int custId)
        {
            return this.customerDataAccess.RMS_CustomerCheckExisting(custId);
        }

        public Result ChangeLevelCustomer(int custId, string userName, int level, int adminId, string ip)
        {
            if (this.CheckExistingCustomer(custId))
            {
                return this.UpdateLevelCustomer(custId, level, adminId, ip);
            }
            else
            {
                return this.AddCustomer(userName, level, adminId, ip);
            }
        }
    }
}