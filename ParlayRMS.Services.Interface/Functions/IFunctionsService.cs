// --------------------------------------------------------------------------
// <copyright file="IFunctionsService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Interface.Functions
{
    using System.Collections.Generic;
    using ParlayRMS.DTO;
    using ParlayRMS.Models.Functions;

    public interface IFunctionsService
    {
        /// <summary>
        /// Creates a new function.
        /// </summary>
        /// <param name="name">Function name.</param>
        /// <param name="key">Function key.</param>
        /// <param name="description">Function description.</param>
        /// <param name="parentId">Parent function ID.</param>
        /// <param name="parentName">Parent function name.</param>
        /// <param name="displayOrder">Display order.</param>
        /// <param name="adminId">ID of user who creates the function.</param>
        /// <param name="ip">IP of machine which creates the function.</param>
        /// <param name="functionId">ID of created function.</param>
        /// <returns>Result indicates whether the function is created successfully or not.</returns>
        Result CreateFunction(
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
        /// <param name="id">ID of function that needs to be updated.</param>
        /// <param name="name">Function name.</param>
        /// <param name="description">Function description.</param>
        /// <param name="parentId">Parent function ID.</param>
        /// <param name="parentName">Parent function name.</param>
        /// <param name="displayOrder">Display order.</param>
        /// <param name="adminId">ID of user who creates the function.</param>
        /// <param name="ip">IP of machine which creates the function.</param>
        /// <returns>Result indicates whether the function is updated successfully or not.</returns>
        Result UpdateFunction(int id, string name, string description, int parentId, string parentName, int displayOrder, int adminId, string ip);

        /// <summary>
        /// Gets all functions.
        /// </summary>
        /// <returns>List of functions.</returns>
        IEnumerable<Function> GetAllFunctions();

        /// <summary>
        /// Gets all functions and order them by DisplayOrder.
        /// </summary>
        /// <returns>List of functions.</returns>
        IEnumerable<Function> GetAllFunctionsInOrder();

        /// <summary>
        /// Gets all functions that are not the child of other functions.
        /// </summary>
        /// <returns>List of functions.</returns>
        IEnumerable<Function> GetRootFunctions();
    }
}