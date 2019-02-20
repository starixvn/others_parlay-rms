// --------------------------------------------------------------------------
// <copyright file="UsersDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.Users
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DataAccess.Implement.Groups;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Users;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Users Data Access.
    /// </summary>
    public class UsersDataAccess : BaseDataAccess
    {
        private readonly GroupsDataAccess _groupDataAccess = new GroupsDataAccess();

        /// <summary>
        /// Gets user by username and group ID.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>User associated with passed username and group ID.</returns>
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

        /// <summary>
        /// Updates user password.
        /// </summary>
        /// <param name="adminId">The ID of administrator.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="password">New password.</param>
        /// <param name="ip">IP of machine which updates the password.</param>
        /// <returns>Whether or not the password is updated successfully.</returns>
        public Result RMS_UserInfoUpdUserPwd(int adminId, int userId, string password, string ip)
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

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="adminId">The ID of administrator who creates the user.</param>
        /// <param name="userName">Username value.</param>
        /// <param name="password">Default password.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <param name="ip">IP of machine which creates user.</param>
        /// <returns>Whether or not the user is created successfully.</returns>
        public Result RMS_UserInfoAddNew(int adminId, string userName, string password, string firstName, string lastName, int groupId, string ip)
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

        /// <summary>
        /// Updates user information.
        /// </summary>
        /// <param name="adminId">The ID of administrator who updates the user.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="ip">IP of machine which updates user.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <returns>Whether or not the user is updated successfully.</returns>
        public Result RMS_UserInfoUpdAll(int adminId, int userId, string firstName, string lastName, string ip, int groupId)
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

        /// <summary>
        /// Updates user status.
        /// </summary>
        /// <param name="adminId">The ID of administrator who creates the user's status.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="active">Whether the user is activated or not.</param>
        /// <param name="ip">IP of machine which updates user's status.</param>
        /// <returns>Whether or not the user's status is updated successfully.</returns>
        public Result RMS_UserInfoUpdStatus(int adminId, int userId, bool active, string ip)
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

        /// <summary>
        /// Adds an existing user to the system.
        /// </summary>
        /// <param name="adminId">The ID of administrator who adds the user.</param>
        /// <param name="userName">Username value.</param>
        /// <param name="groupId">Group ID value.</param>
        /// <param name="ip">IP of machine which adds user.</param>
        /// <returns>Whether or not the user is added successfully.</returns>
        public Result RMS_UserInfoAddExisting(int adminId, string userName, int groupId, string ip)
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

        /// <summary>
        /// Gets user's policy.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>Policy of user.</returns>
        public IEnumerable<RMS_UserPolicySel> RMS_UserPolicySel(int userId)
        {
            var result = new List<RMS_UserPolicySel>();

            var htb = new Hashtable();
            htb.Add("userid", userId);

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

        /// <summary>
        /// Updates user's policy.
        /// </summary>
        /// <param name="adminId">The ID of administrator who updates the user's policy.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="policy">New policy.</param>
        /// <param name="ip">IP of machine which updates user's policy.</param>
        /// <returns>Whether or not the user's policy is updated successfully.</returns>
        public Result RMS_UserPolicyUpd(int adminId, int userId, string policy, string ip)
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

        /// <summary>
        /// Gets available menus of a user.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>List of menus and functions that are available to the user.</returns>
        public RMS_MenuSelByUserID RMS_MenuSelByUserID(int userId)
        {
            Hashtable param = new Hashtable();
            param.Add("userid", userId);
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

        /// <summary>
        /// Gets user information by user ID.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>User information.</returns>
        public RMS_UserInfoSelByUserID RMS_UserInfoSelByUserID(int userId)
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

            return null;
        }

        /// <summary>
        /// Validates user login information.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <param name="password">Password value.</param>
        /// <param name="loginIp">The IP of machine which the user using to log in.</param>
        /// <param name="loginDomain">Login domain.</param>
        /// <param name="browserName">The name of browser which the user using to log in.</param>
        /// <param name="platform">Platform of machine which the user using to log in.</param>
        /// <param name="countryCode">Country code of place where the machine is staying.</param>
        /// <param name="language">Language ID.</param>
        /// <param name="result">Whether or not the passed information indicates a valid user.</param>
        /// <returns>User Information.</returns>
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

            return null;
        }

        /// <summary>
        /// ReLogin process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <returns>Whether or re-login successfully.</returns>
        public bool RMS_UserRecheckIn(int userId)
        {
            Hashtable param = new Hashtable();
            param.Add("on_userid", userId);
            param.Add("iskickedout", 0);

            var r = ExecSp("RMS_UserRecheckIn", param);
            if (r.IsValid())
            {
                return DbConverter.ToBoolean(r.Outputs["iskickedout"]);
            }

            return true;
        }

        /// <summary>
        /// Check in process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="netSessionId">Session ID value.</param>
        /// <param name="ip">The IP of machine which the user using to log out.</param>
        /// <returns>Whether or not the user logged out successful.</returns>
        public int RMS_UserCheckIn(int userId, string netSessionId, string ip)
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

            return -1;
        }

        /// <summary>
        /// Logout process.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="userName">Username value.</param>
        public void RMS_UserLogout(int userId, string userName)
        {
            Hashtable param = new Hashtable();
            param.Add("on_userid", userId);

            this.ExecSp("RMS_UserLogout", param);
        }

        /// <summary>
        /// Gets user information by username.
        /// </summary>
        /// <param name="username">Username value.</param>
        /// <returns>User information.</returns>
        public RMS_CustomerSelInfoByUsername RMS_CustomerSelInfoByUsername(string username)
        {
            Hashtable param = new Hashtable();
            param.Add("username", username);
            param.Add("custid", 0);
            param.Add("roleid", 0);
            var r = ExecSp("RMS_CustomerSelInfoByUsername", param);

            return new RMS_CustomerSelInfoByUsername
            {
                Username = username,
                CustId = DbConverter.ToInt64(r.Outputs["custid"]),
                RoleId = DbConverter.ToInt32(r.Outputs["roleid"])
            };
        }
    }
}