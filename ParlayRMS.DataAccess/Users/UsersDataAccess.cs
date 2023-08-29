namespace ParlayRMS.DataAccess.Users
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Groups;
    using DTO;
    using ParlayRMS.DTO.Users;
    using Utility;
    using Utility.Data;

    public class UsersDataAccess : BaseDataAccess
    {
        private readonly GroupsDataAccess _groupDataAccess = new GroupsDataAccess();

        public IEnumerable<RMS_UserInfoSel> RMS_UserInfoSel(string userName, int groupId)
        {
            var result = new List<RMS_UserInfoSel>();

            var param = new Hashtable();
            if (groupId > 0)
            {
                param.Add("groupid", groupId);
            }

            if (!string.IsNullOrEmpty(userName))
            {
                param.Add("username", userName);
            }

            using (var reader = ExecSpDataReader("RMS_UserInfoSel", param))
            {
                while (reader.Read())
                {
                    var user = new RMS_UserInfoSel
                    {
                        CustId = reader.ToInt32("custid"),
                        UserName = reader.ToString("username"),
                        FirstName = reader.ToString("firstname"),
                        LastName = reader.ToString("lastname"),
                        GroupId = reader.ToInt32("groupid"),
                        GroupName = reader.ToString("groupname"),
                        Creator = reader.ToString("creator"),
                        CreatedDate = reader.ToDateTime("createddate"),
                        Enabled = reader.ToBoolean("enabled")
                    };

                    result.Add(user);
                }
            }

            return result;
        }

        public Result RMS_UserInfoUpdUserPwd(int adminId, int userId, string password, string ip)
        {
            var htb = new Hashtable
            {
                { "adminid", adminId },
                { "custid", userId },
                { "userpwd", Cryptography.CfsEnCode(password) },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_UserInfoUpdUserPwd", htb);

            return r.ToResult();
        }

        public Result RMS_UserInfoAddNew(int adminId, string userName, string password, string firstName, string lastName, int groupId, string ip)
        {
            var htb = new Hashtable
            {
                { "adminid", adminId },
                { "username", userName },
                { "custid", 0 },
                { "userpwd", Cryptography.CfsEnCode(password) },
                { "firstname", firstName },
                { "lastname", lastName },
                { "groupid", groupId },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_UserInfoAddNew", htb);

            return r.ToResult();
        }

        public Result RMS_UserInfoUpdAll(int adminId, int userId, string firstName, string lastName, string ip, int groupId)
        {
            var htb = new Hashtable
            {
                { "adminid", adminId },
                { "custid", userId },
                { "firstname", firstName },
                { "lastname", lastName },
                { "ip", ip },
                { "groupid", groupId },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_UserInfoUpdAll", htb);

            return r.ToResult();
        }

        public Result RMS_UserInfoUpdStatus(int adminId, int userId, bool active, string ip)
        {
            var htb = new Hashtable
            {
                { "adminid", adminId },
                { "custid", userId },
                { "enabled", active },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_UserInfoUpdStatus", htb);

            return r.ToResult();
        }

        public Result RMS_UserInfoAddExisting(int adminId, string userName, int groupId, string ip)
        {
            var htb = new Hashtable
            {
                { "adminid", adminId },
                { "username", userName },
                { "custid", 0 },
                { "groupid", groupId },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_UserInfoAddExisting", htb);

            return r.ToResult();
        }

        public IEnumerable<RMS_UserPolicySel> RMS_UserPolicySel(int userId)
        {
            var result = new List<RMS_UserPolicySel>();

            var htb = new Hashtable { { "userid", userId } };

            using (var reader = ExecSpDataReader("RMS_UserPolicySel", htb))
            {
                while (reader.Read())
                {
                    result.Add(new RMS_UserPolicySel
                    {
                        FuncId = reader.ToInt32("funcid"),
                        FuncKey = reader.ToString("funckey"),
                        FuncName = reader.ToString("funcname"),
                        ParentId = reader.ToInt32("parentid"),
                        ParentName = reader.ToString("parentname"),
                        UserPolicy = reader.ToInt32("userpolicy"),
                        GroupPolicy = reader.ToInt32("grouppolicy"),
                        DisplayOrder = reader.ToInt32("displayorder")
                    });
                }
            }

            return result;
        }

        public Result RMS_UserPolicyUpd(int adminId, int userId, string policy, string ip)
        {
            var htb = new Hashtable
            {
                { "adminid", adminId },
                { "custid", userId },
                { "userpolicy", policy },
                { "ip", ip },
                { "errcode", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_UserPolicyUpd", htb);

            return r.ToResult();
        }

        public RMS_MenuSelByUserID RMS_MenuSelByUserID(int userId)
        {
            var param = new Hashtable { { "userid", userId } };
            var ds = ExecSpDataSet("RMS_MenuSelByUserID", param);

            var menus = from dr in ds.Tables[0].AsEnumerable()
                        select new RMS_MenuSelByUserID_Menu
                        {
                            ParentId = dr.ToInt32("parentid"),
                            ParentName = dr.ToString("parentname"),
                            MenuId = dr.ToInt32("menuid"),
                            MenuName = dr.ToString("menuname"),
                            Url = dr.ToString("Url"),
                            DisplayOrder = dr.ToInt32("displayorder")
                        };

            var functions = from dr in ds.Tables[1].AsEnumerable()
                            select new RMS_MenuSelByUserID_Function
                            {
                                FuncId = dr.ToInt32("funcid"),
                                FuncName = dr.ToString("funcname"),
                                FuncKey = dr.ToString("funckey"),
                                Policy = dr.ToInt32("policy")
                            };

            return new RMS_MenuSelByUserID
            {
                Menus = menus,
                Functions = functions
            };
        }

        public RMS_UserInfoSelByUserID RMS_UserInfoSelByUserID(int userId)
        {
            var htb = new Hashtable { { "userid", userId } };
            using (var reader = ExecSpDataReader("RMS_UserInfoSelByUserID", htb))
            {
                while (reader.Read())
                {
                    return new RMS_UserInfoSelByUserID
                    {
                        CustId = userId,
                        UserName = reader.ToString("username"),
                        FirstName = reader.ToString("firstname"),
                        LastName = reader.ToString("lastname"),
                        Enabled = reader.ToBoolean("enabled"),
                        GroupId = reader.ToInt32("groupid"),
                        GroupName = reader.ToString("groupname")
                    };
                }
            }

            return null;
        }

        public RMS_UserInfoSelByUserID RMS_UserLogin(
                                           string userName,
                                           string password,
                                           string loginIp,
                                           string loginDomain,
                                           string browserName,
                                           string platform,
                                           string countryCode,
                                           string language,
                                           out Result result)
        {
            result = Result.Empty;

            var param = new Hashtable
            {
                { "username", userName },
                { "userpwd", Cryptography.CfsEnCode(password) },
                { "loginip", loginIp },
                { "logindomain", loginDomain },
                { "browsername", browserName },
                { "platform", platform },
                { "countrycode", countryCode },
                { "language", language },
                { "errcode", 0 },
                { "custid", 0 },
                { "errmsg", string.Empty }
            };

            var r = ExecSp("RMS_UserLogin", param);
            result = r.ToResult();
            if (result.IsSuccessful)
            {
                return RMS_UserInfoSelByUserID(DbConverter.ToInt32(r.Outputs["custid"]));
            }

            return null;
        }

        public bool RMS_UserRecheckIn(int userId)
        {
            var param = new Hashtable
            {
                { "on_userid", userId },
                { "iskickedout", 0 }
            };

            var r = ExecSp("RMS_UserRecheckIn", param);
            if (r.IsValid())
            {
                return DbConverter.ToBoolean(r.Outputs["iskickedout"]);
            }

            return true;
        }

        public int RMS_UserCheckIn(int userId, string netSessionId, string ip)
        {
            var param = new Hashtable
            {
                { "custid", userId },
                { "netsessionid", netSessionId },
                { "ip", ip },
                { "on_userid", 0 }
            };

            var r = ExecSp("RMS_UserCheckIn", param);
            if (r.IsValid())
            {
                return DbConverter.ToInt32(r.Outputs["on_userid"]);
            }

            return -1;
        }

        public void RMS_UserLogout(int userId, string userName)
        {
            var param = new Hashtable { { "on_userid", userId } };

            ExecSp("RMS_UserLogout", param);
        }

        public RMS_CustomerSelInfoByUsername RMS_CustomerSelInfoByUsername(string username)
        {
            var param = new Hashtable
            {
                { "username", username },
                { "custid", 0 },
                { "roleid", 0 }
            };
            var r = ExecSp("RMS_CustomerSelInfoByUsername", param);

            return new RMS_CustomerSelInfoByUsername
            {
                Username = username,
                CustId = DbConverter.ToInt64(r.Outputs["custid"]),
                RoleId = DbConverter.ToInt32(r.Outputs["roleid"])
            };
        }



        public IEnumerable<int> RMS_CustomerAgentOfMasterSel(int masterId)
        {
            var htb = new Hashtable { { "custid", masterId } };
            using (var reader = ExecSpDataReader("RMS_CustomerAgentOfMasterSel", htb))
            {
                while (reader.Read())
                {
                    yield return reader.ToInt32("custid");
                }
            }
        }
    }
}
