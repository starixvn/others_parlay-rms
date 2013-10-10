// --------------------------------------------------------------------------
// <copyright file="IMembersDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.Members
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Members;

    public interface IMembersDataAccess
    {
        /// <summary>
        /// Gets customer by username and level.
        /// </summary>
        /// <param name="username">Username value.</param>
        /// <param name="level">Level value.</param>
        /// <returns>List of customers.</returns>
        IEnumerable<RMS_CustomerSel> RMS_CustomerSel(string username, int? level);

        /// <summary>
        /// Inserts a new customer.
        /// </summary>
        /// <param name="userName">Username value.</param>
        /// <param name="level">Level value.</param>
        /// <param name="adminId">The ID of administrator who creates the customer.</param>
        /// <param name="ip">IP of machine which creates the customer.</param>
        /// <returns>Whether or not customer is created successfully.</returns>
        Result RMS_CustomerIns(string userName, int level, int adminId, string ip);

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="level">Level value.</param>
        /// <param name="adminId">The ID of administrator who updates the customer.</param>
        /// <param name="ip">IP of machine which updates the customer.</param>
        /// <returns>Whether or not customer is updated successfully.</returns>
        Result RMS_CustomerUpd(int custId, int level, int adminId, string ip);

        /// <summary>
        /// Updates a customer status.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <param name="following">Is monitored or not.</param>
        /// <param name="adminId">The ID of administrator who updates the customer.</param>
        /// <param name="ip">IP of machine which updates the customer.</param>
        /// <returns>Whether or not customer is updated successfully.</returns>
        Result RMS_CustomerUpdStatus(int custId, bool following, int adminId, string ip);

        /// <summary>
        /// Checks a customer is existing or not.
        /// </summary>
        /// <param name="custId">Customer ID.</param>
        /// <returns>Whether or not customer is existing.</returns>
        bool RMS_CustomerCheckExisting(int custId);
    }
}