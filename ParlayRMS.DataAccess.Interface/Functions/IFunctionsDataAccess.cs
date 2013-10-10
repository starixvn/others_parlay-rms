// --------------------------------------------------------------------------
// <copyright file="IFunctionsDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Interface.Functions
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Functions;

    public interface IFunctionsDataAccess
    {
        /// <summary>
        /// Inserts a new function.
        /// </summary>
        /// <param name="name">Function name.</param>
        /// <param name="key">Function key.</param>
        /// <param name="description">Function description.</param>
        /// <param name="parentId">Parent function ID.</param>
        /// <param name="parentName">Parent function name.</param>
        /// <param name="displayOrder">Display order.</param>
        /// <param name="adminId">User who creates function.</param>
        /// <param name="ip">IP of machine which creates function.</param>
        /// <param name="functionId">Function ID.</param>
        /// <returns>Whether or not function is created successfully.</returns>
        Result RMS_FunctionIns(
                   string name,
                   string key,
                   string description,
                   int parentId,
                   string parentName,
                   int displayOrder,
                   int adminId,
                   string ip,
                   out int functionId);

        /// <summary>
        /// Updates an existing function.
        /// </summary>
        /// <param name="id">Function ID.</param>
        /// <param name="name">Function name.</param>
        /// <param name="description">Function description.</param>
        /// <param name="parentId">Parent function ID.</param>
        /// <param name="parentName">Parent function name.</param>
        /// <param name="displayOrder">Display order.</param>
        /// <param name="adminId">User who updates function.</param>
        /// <param name="ip">IP of machine which updates function.</param>
        /// <returns>Whether or not function is updated successfully.</returns>
        Result RMS_FunctionUpd(int id, string name, string description, int parentId, string parentName, int displayOrder, int adminId, string ip);

        /// <summary>
        /// Gets list of functions.
        /// </summary>
        /// <returns>List of functions.</returns>
        IEnumerable<RMS_FunctionSel> RMS_FunctionSel();
    }
}