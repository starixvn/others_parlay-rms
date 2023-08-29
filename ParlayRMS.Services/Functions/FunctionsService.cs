// --------------------------------------------------------------------------
// <copyright file="FunctionsService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Services.Functions
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Functions;
    using Models.Functions;
    using ParlayRMS.DTO;

    /// <summary>
    /// Functions Service.
    /// </summary>
    public class FunctionsService : BaseService
    {
        private readonly FunctionsDataAccess _functionsDataAccess = new FunctionsDataAccess();

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
        public Result CreateFunction(
                          string name,
                          string key,
                          string description,
                          int parentId,
                          string parentName,
                          int displayOrder,
                          int adminId,
                          string ip,
                          out int functionId)
        {
            return this._functionsDataAccess.RMS_FunctionIns(name, key, description, parentId, parentName, displayOrder, adminId, ip, out functionId);
        }

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
        public Result UpdateFunction(int id, string name, string description, int parentId, string parentName, int displayOrder, int adminId, string ip)
        {
            return this._functionsDataAccess.RMS_FunctionUpd(id, name, description, parentId, parentName, displayOrder, adminId, ip);
        }

        /// <summary>
        /// Gets all functions.
        /// </summary>
        /// <returns>List of functions.</returns>
        public IEnumerable<Function> GetAllFunctions()
        {
            var data = this._functionsDataAccess.RMS_FunctionSel();
            return data.Select(x => new Function
            {
                Id = x.FuncId,
                Name = x.FuncName,
                Key = x.FuncKey,
                Description = x.FuncDesc,
                Parent = x.ParentId == 0 ? null : new Function
                {
                    Id = x.ParentId,
                    Name = x.ParentName
                },
                DisplayOrder = x.DisplayOrder,
                ModifiedDate = x.ModifiedDate
            });
        }

        /// <summary>
        /// Gets all functions and order them by DisplayOrder.
        /// </summary>
        /// <returns>List of functions.</returns>
        public IEnumerable<Function> GetAllFunctionsInOrder()
        {
            var functionList = this.GetAllFunctions();
            var parents = functionList.Where(x => x.Parent == null);
            var list = new List<Function>();
            foreach (var parent in parents)
            {
                list.Add(parent);
                var items = functionList.Where(x => x.Parent != null && x.Parent.Id == parent.Id);
                if (items.Count() > 0)
                {
                    list.AddRange(items);
                }
            }

            return list;
        }

        /// <summary>
        /// Gets all functions that are not the child of other functions.
        /// </summary>
        /// <returns>List of functions.</returns>
        public IEnumerable<Function> GetRootFunctions()
        {
            var functionList = this.GetAllFunctions();
            var rootFunctionList = functionList.Where(x => x.Parent == null);
            return rootFunctionList;
        }
    }
}