namespace ParlayRMS.DataAccess.Members
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using DTO;
    using ParlayRMS.DTO.Members;
    using Utility;
    using Utility.Data;

    public class MembersDataAccess : BaseDataAccess
    {
        public IEnumerable<RMS_CustomerSel> RMS_CustomerSel(string username, int? level)
        {
            var result = new List<RMS_CustomerSel>();

            using (var db = new ObjectDb("RMS_CustomerSel"))
            {
                if (!string.IsNullOrEmpty(username))
                {
                    db.AddInParameter("username", DbType.String, username);
                }

                if (level != null)
                {
                    db.AddInParameter("level", DbType.Int32, level);
                }

                var counter = 1;
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_CustomerSel
                        {
                            No = counter++,
                            Site = reader.ToString("SiteGroup"),
                            Custid = reader.ToInt32("CustId"),
                            Username = reader.ToString("UserName"),
                            Level = reader.ToInt32("Level")
                        });
                    }
                }
            }

            return result;
        }

        public Result RMS_CustomerIns(string userName, int level, int adminId, string ip)
        {
            var param = new Hashtable
            {
                { "adminid", adminId },
                { "username", userName },
                { "level", level },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_CustomerIns", param);

            return r.ToResult();
        }

        public Result RMS_CustomerUpd(int custId, int level, int adminId, string ip)
        {
            var param = new Hashtable
            {
                { "adminid", adminId },
                { "custid", custId },
                { "level", level },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_CustomerUpd", param);

            return r.ToResult();
        }

        public Result RMS_CustomerUpdStatus(int custId, bool following, int adminId, string ip)
        {
            var param = new Hashtable
            {
                { "adminid", adminId },
                { "custid", custId },
                { "monitored", following },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_CustomerUpdStatus", param);

            return r.ToResult();
        }

        public bool RMS_CustomerCheckExisting(int custId)
        {
            var param = new Hashtable
            {
                { "custid", custId },
                { "isexists", 0 }
            };

            var r = ExecSp("RMS_CustomerCheckExisting", param);
            if (r.IsValid())
            {
                return DbConverter.ToBoolean(r.Outputs["isexists"]);
            }

            return false;
        }
    }
}
