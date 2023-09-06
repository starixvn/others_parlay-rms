namespace ParlayRMS.Services.Members
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using DataAccess;
    using ParlayRMS.DTO.Members;

    public class MembersService : BaseService
    {
        private readonly MembersDataAccess _customerDataAccess = new MembersDataAccess();

        public IEnumerable<RMS_CustomerSel> GetListCustomer(string username, int? level)
        {
            return _customerDataAccess.RMS_CustomerSel(username, level);
        }

        public Result AddCustomer(string userName, int level, int adminId, string ip)
        {
            return _customerDataAccess.RMS_CustomerIns(userName, level, adminId, ip);
        }

        public Result UpdateLevelCustomer(int custId, int level, int adminId, string ip)
        {
            return _customerDataAccess.RMS_CustomerUpd(custId, level, adminId, ip);
        }

        public Result UpdateStatusCustomer(int custId, bool following, int adminId, string ip)
        {
            return _customerDataAccess.RMS_CustomerUpdStatus(custId, following, adminId, ip);
        }

        public bool CheckExistingCustomer(int custId)
        {
            return _customerDataAccess.RMS_CustomerCheckExisting(custId);
        }

        public Result ChangeLevelCustomer(int custId, string userName, int level, int adminId, string ip)
        {
            return CheckExistingCustomer(custId) ? UpdateLevelCustomer(custId, level, adminId, ip) : AddCustomer(userName, level, adminId, ip);
        }

        public List<int> GetAgents(int masterId)
        {
            return _customerDataAccess.RMS_CustomerAgentOfMasterSel(masterId).ToList();
        }

        public RMS_CustomerSelInfoByUsername GetCustomerInfo(string username)
        {
            return _customerDataAccess.RMS_CustomerSelInfoByUsername(username);
        }
    }
}
