// --------------------------------------------------------------------------
// <copyright file="MembersDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.Members
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Members;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Member Data Access.
    /// </summary>
    public class MembersDataAccess : BaseDataAccess
    {
        /// <summary>
        /// Gets customer by username and level.
        /// </summary>
        /// <param name="username">Username value.</param>
        /// <param name="level">Level value.</param>
        /// <returns>List of customers.</returns>
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

                int counter = 1;
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

        /// <summary>
        /// Inserts a new customer.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <param name="level">Level value.</param>
        /// <param name="adminId">The ID of administrator who creates the customer.</param>
        /// <param name="ip">IP of machine which creates the customer.</param>
        /// <returns>Whether or not customer is created successfully.</returns>
        public Result RMS_CustomerIns(string userName, int level, int adminId, string ip)
        {
            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("username", userName);
            param.Add("level", level);
            param.Add("ip", ip);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            var r = ExecSp("RMS_CustomerIns", param);

            return r.ToResult();
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="level">Level value.</param>
        /// <param name="adminId">The ID of administrator who updates the customer.</param>
        /// <param name="ip">IP of machine which updates the customer.</param>
        /// <returns>Whether or not customer is updated successfully.</returns>
        public Result RMS_CustomerUpd(int custId, int level, int adminId, string ip)
        {
            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("custid", custId);
            param.Add("level", level);
            param.Add("ip", ip);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            var r = ExecSp("RMS_CustomerUpd", param);

            return r.ToResult();
        }

        /// <summary>
        /// Updates a customer status.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="following">Is monitored or not.</param>
        /// <param name="adminId">The ID of administrator who updates the customer.</param>
        /// <param name="ip">IP of machine which updates the customer.</param>
        /// <returns>Whether or not customer is updated successfully.</returns>
        public Result RMS_CustomerUpdStatus(int custId, bool following, int adminId, string ip)
        {
            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("custid", custId);
            param.Add("monitored", following);
            param.Add("ip", ip);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);

            var r = ExecSp("RMS_CustomerUpdStatus", param);

            return r.ToResult();
        }

        /// <summary>
        /// Checks a customer is existing or not.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <returns>Whether or not customer is existing.</returns>
        public bool RMS_CustomerCheckExisting(int custId)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            param.Add("isexists", 0);

            var r = ExecSp("RMS_CustomerCheckExisting", param);
            if (r.IsValid())
            {
                return DbConverter.ToBoolean(r.Outputs["isexists"]);
            }

            return false;
        }
    }
}