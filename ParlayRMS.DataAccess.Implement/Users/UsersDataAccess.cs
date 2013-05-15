namespace ParlayRMS.DataAccess.Implement.Users
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DataAccess.Implement.Groups;
    using ParlayRMS.DataAccess.Interface.Groups;
    using ParlayRMS.DataAccess.Interface.Users;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;

    public class UsersDataAccess : BaseDataAccess, IUsersDataAccess
    {
        private readonly IGroupsDataAccess groupDataAccess = new GroupsDataAccess();

        /// <summary>
        ///
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_UserInfoSel> RMS_UserInfoSel(string userName, int groupId)
        {
            var result = new List<RMS_UserInfoSel>();

            try
            {
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
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Result RMS_UserInfoUpdUserPwd(int adminId, int userId, string password, string ip)
        {
            try
            {
                var htb = new Hashtable();
                htb.Add("adminid", adminId);
                htb.Add("custid", userId);
                htb.Add("userpwd", Cryptography.CfsEnCode(password));
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                var r = ExecSp("RMS_UserInfoUpdUserPwd", htb);
                return r.ToResult();
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return Result.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="groupId"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Result RMS_UserInfoAddNew(int adminId, string userName, string password, string firstName, string lastName, int groupId, string ip)
        {
            try
            {
                var htb = new Hashtable();
                htb.Add("adminid", adminId);
                htb.Add("username", userName);
                htb.Add("custid", 0);
                htb.Add("userpwd", Cryptography.CfsEnCode(password));
                htb.Add("firstname", firstName);
                htb.Add("lastname", lastName);
                htb.Add("groupid", groupId);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                var r = ExecSp("RMS_UserInfoAddNew", htb);
                return r.ToResult();
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return Result.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="ip"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public Result RMS_UserInfoUpdAll(int adminId, int userId, string firstName, string lastName, string ip, int groupId)
        {
            try
            {
                var htb = new Hashtable();
                htb.Add("adminid", adminId);
                htb.Add("custid", userId);
                htb.Add("firstname", firstName);
                htb.Add("lastname", lastName);
                htb.Add("ip", ip);
                htb.Add("groupid", groupId);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                var r = ExecSp("RMS_UserInfoUpdAll", htb);
                return r.ToResult();
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return Result.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        /// <param name="active"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Result RMS_UserInfoUpdStatus(int adminId, int userId, bool active, string ip)
        {
            try
            {
                var htb = new Hashtable();
                htb.Add("adminid", adminId);
                htb.Add("custid", userId);
                htb.Add("enabled", active);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                var r = ExecSp("RMS_UserInfoUpdStatus", htb);
                return r.ToResult();
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return Result.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userName"></param>
        /// <param name="groupId"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Result RMS_UserInfoAddExisting(int adminId, string userName, int groupId, string ip)
        {
            try
            {
                var htb = new Hashtable();
                htb.Add("adminid", adminId);
                htb.Add("username", userName);
                htb.Add("custid", 0);
                htb.Add("groupid", groupId);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                var r = ExecSp("RMS_UserInfoAddExisting", htb);
                return r.ToResult();
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return Result.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<RMS_UserPolicySel> RMS_UserPolicySel(int userId)
        {
            var result = new List<RMS_UserPolicySel>();

            try
            {
                var htb = new Hashtable();
                htb.Add("userid", userId);

                using (var reader = ExecSpDataReader("RMS_UserPolicySel", htb))
                {
                    while (reader.Read())
                    {
                        result.Add(new RMS_UserPolicySel
                        {
                            FuncId = reader.ToInt32("funcid"),
                            FuncName = reader.ToString("funcname"),
                            ParentId = reader.ToInt32("parentid"),
                            ParentName = reader.ToString("parentname"),
                            UserPolicy = reader.ToInt32("userpolicy"),
                            GroupPolicy = reader.ToInt32("grouppolicy"),
                            DisplayOrder = reader.ToInt32("displayorder")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="userId"></param>
        /// <param name="policy"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Result RMS_UserPolicyUpd(int adminId, int userId, string policy, string ip)
        {
            try
            {
                var htb = new Hashtable();
                htb.Add("adminid", adminId);
                htb.Add("custid", userId);
                htb.Add("userpolicy", policy);
                htb.Add("ip", ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", string.Empty);

                var r = ExecSp("RMS_UserPolicyUpd", htb);
                return r.ToResult();
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return Result.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RMS_MenuSelByUserID RMS_MenuSelByUserID(int userId)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", userId);
                var ds = ExecSpDataSet("RMS_MenuSelByUserID", param);

                var menus = (from dr in ds.Tables[0].AsEnumerable()
                             select new RMS_MenuSelByUserID_Menu
                                {
                                    ParentId = dr.ToInt32("parentid"),
                                    ParentName = dr.ToString("parentname"),
                                    MenuId = dr.ToInt32("menuid"),
                                    MenuName = dr.ToString("menuname"),
                                    Url = dr.ToString("Url"),
                                    DisplayOrder = dr.ToInt32("displayorder")
                                });

                var functions = (from dr in ds.Tables[1].AsEnumerable()
                                 select new RMS_MenuSelByUserID_Function
                                  {
                                      FuncId = dr.ToInt32("funcid"),
                                      FuncName = dr.ToString("funcname"),
                                      FuncKey = dr.ToString("funckey"),
                                      Policy = dr.ToInt32("policy")
                                  });

                return new RMS_MenuSelByUserID
                {
                    Menus = menus,
                    Functions = functions
                };
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return null;
        }

        public RMS_UserInfoSelByUserID RMS_UserInfoSelByUserID(int userId)
        {
            try
            {
                var htb = new Hashtable();
                htb.Add("userid", userId);
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
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return null;
        }

        /// <summary>
        /// Validate user process
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="netSessionId"></param>
        /// <param name="request"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public RMS_UserInfoSelByUserID RMS_UserLogin(string userName, string password, string loginIp, string loginDomain, string browserName, string platform, string countryCode, string language, out Result result)
        {
            result = Result.Empty;

            try
            {
                Hashtable param = new Hashtable();
                param.Add("username", userName);
                param.Add("userpwd", Cryptography.CfsEnCode(password));
                param.Add("loginip", loginIp);
                param.Add("logindomain", loginDomain);
                param.Add("browsername", browserName);
                param.Add("platform", platform);
                param.Add("countrycode", countryCode);
                param.Add("language", language);
                param.Add("errcode", 0);
                param.Add("custid", 0);
                param.Add("errmsg", string.Empty);

                var r = ExecSp("RMS_UserLogin", param);
                result = r.ToResult();
                if (result.IsSuccessful)
                {
                    return this.RMS_UserInfoSelByUserID(DbConverter.ToInt32(r.Outputs["custid"]));
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return null;
        }

        /// <summary>
        /// ReLogin process
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool RMS_UserRecheckIn(int userId)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("on_userid", userId);
                param.Add("iskickedout", 0);

                var r = ExecSp("RMS_UserRecheckIn", param);
                if (r.IsValid())
                {
                    return DbConverter.ToBoolean(r.Outputs["iskickedout"]);
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return true;
        }

        /// <summary>
        /// Login onUser process
        /// </summary>
        /// <param name="user"></param>
        /// <param name="netSessionId"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int RMS_UserCheckIn(int userId, string netSessionId, string ip)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("custid", userId);
                param.Add("netsessionid", netSessionId);
                param.Add("ip", ip);
                param.Add("on_userid", 0);

                var r = ExecSp("RMS_UserCheckIn", param);
                if (r.IsValid())
                {
                    return DbConverter.ToInt32(r.Outputs["on_userid"]);
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return -1;
        }

        /// <summary>
        /// Logout process
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        public void RMS_UserLogout(int userId, string userName)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("on_userid", userId);
                param.Add("username", userName);

                this.ExecSp("RMS_UserLogout", param);
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }
        }

        public RMS_CustomerSelInfoByUsername RMS_CustomerSelInfoByUsername(string username)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("username", username);
                var r = ExecSp("RMS_CustomerSelInfoByUsername", param);
                return new RMS_CustomerSelInfoByUsername
                {
                    Username = username,
                    CustId = DbConverter.ToInt64(r.Outputs["custid"]),
                    RoleId = DbConverter.ToInt32(r.Outputs["roleid"])
                };
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return null;
        }
    }
}