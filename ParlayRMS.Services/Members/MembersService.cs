// --------------------------------------------------------------------------
// <copyright file="MembersService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Members
{
    using System.Collections.Generic;
    using DataAccess.Members;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Members;

    /// <summary>
    /// Member Service.
    /// </summary>
    public class MembersService : BaseService
    {
        private readonly MembersDataAccess _customerDataAccess = new MembersDataAccess();

        /// <summary>
        /// Gets a customer by username and level.
        /// </summary>
        /// <param name="username">Username value.</param>
        /// <param name="level">Level value.</param>
        /// <returns>List of customers.</returns>
        public IEnumerable<RMS_CustomerSel> GetListCustomer(string username, int? level)
        {
            return this._customerDataAccess.RMS_CustomerSel(username, level);
        }

        /// <summary>
        /// Inserts a new customer.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <param name="level">Level value.</param>
        /// <param name="adminId">The ID of administrator who creates the customer.</param>
        /// <param name="ip">IP of machine which creates the customer.</param>
        /// <returns>Whether or not customer is created successfully.</returns>
        public Result AddCustomer(string userName, int level, int adminId, string ip)
        {
            return this._customerDataAccess.RMS_CustomerIns(userName, level, adminId, ip);
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="level">Level value.</param>
        /// <param name="adminId">The ID of administrator who updates the customer.</param>
        /// <param name="ip">IP of machine which updates the customer.</param>
        /// <returns>Whether or not customer is updated successfully.</returns>
        public Result UpdateLevelCustomer(int custId, int level, int adminId, string ip)
        {
            return this._customerDataAccess.RMS_CustomerUpd(custId, level, adminId, ip);
        }

        /// <summary>
        /// Updates a customer status.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="following">Is monitored or not.</param>
        /// <param name="adminId">The ID of administrator who updates the customer.</param>
        /// <param name="ip">IP of machine which updates the customer.</param>
        /// <returns>Whether or not customer is updated successfully.</returns>
        public Result UpdateStatusCustomer(int custId, bool following, int adminId, string ip)
        {
            return this._customerDataAccess.RMS_CustomerUpdStatus(custId, following, adminId, ip);
        }

        /// <summary>
        /// Checks a customer is existing or not.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <returns>Whether or not customer is existing.</returns>
        public bool CheckExistingCustomer(int custId)
        {
            return this._customerDataAccess.RMS_CustomerCheckExisting(custId);
        }

        /// <summary>
        /// Changes a customer level.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="userName">Username value.</param>
        /// <param name="level">Level value.</param>
        /// <param name="adminId">The ID of administrator who updates the customer.</param>
        /// <param name="ip">IP of machine which updates the customer.</param>
        /// <returns>Whether or not customer is updated successfully.</returns>
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