namespace ParlayRMS.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Criteria;
    using Criteria.Members;
    using DTO.Members;

    public class MembersDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public IEnumerable<RMS_CustomerSel> RMS_CustomerSel(string username, int? level)
        {
            var criteria = new CustomerSel_Criteria
            {
                Username = string.IsNullOrEmpty(username) ? null : username,
                Level = level
            };

            var members = repository.Fetch<RMS_CustomerSel>(criteria).ToList();

            var index = 1;
            members.ForEach(m => m.No = index++);

            return members;
        }

        public Result RMS_CustomerIns(string userName, int level, int adminId, string ip)
        {
            var command = new CustomerIns_Command
            {
                AdminId = adminId,
                Username = userName,
                Level = level,
                Ip = ip
            };

            repository.Execute(command);

            return ResultFactory.From(command);
        }

        public Result RMS_CustomerUpd(int custId, int level, int adminId, string ip)
        {
            var command = new CustomerUpd_Command
            {
                AdminId = adminId,
                CustId = custId,
                Level = level,
                Ip = ip
            };

            repository.Execute(command);

            return ResultFactory.From(command);
        }

        public Result RMS_CustomerUpdStatus(int custId, bool following, int adminId, string ip)
        {
            var command = new CustomerUpdStatus_Command
            {
                AdminId = adminId,
                CustId = custId,
                Monitored = following,
                Ip = ip
            };

            repository.Execute(command);

            return ResultFactory.From(command);
        }

        public bool RMS_CustomerCheckExisting(int custId)
        {
            var command = new CustomerCheckExisting_Command
            {
                CustId = custId
            };

            repository.Execute(command);

            return command.IsExists;

        }
        

        public IEnumerable<int> RMS_CustomerAgentOfMasterSel(int masterId)
        {
            var criteria = new CustomerAgentOfMasterSel_Criteria { CustId = masterId };
            return repository.Fetch<int>(criteria);
        }


        public RMS_CustomerSelInfoByUsername RMS_CustomerSelInfoByUsername(string username)
        {
            var command = new CustomerSelInfoByUsername_Command
            {
                Username = username
            };

            repository.Execute(command);

            

            return new RMS_CustomerSelInfoByUsername
            {
                Username = username,
                CustId = command.CustId,
                RoleId = command.RoleId
            };
        }

    }
}
