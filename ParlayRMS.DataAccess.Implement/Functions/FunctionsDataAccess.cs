// --------------------------------------------------------------------------
// <copyright file="FunctionsDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DataAccess.Implement.Functions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DataAccess.Interface.Functions;
    using ParlayRMS.DTO;
    using ParlayRMS.DTO.Functions;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;

    /// <summary>
    /// Function Data Access.
    /// </summary>
    public class FunctionsDataAccess : BaseDataAccess, IFunctionsDataAccess
    {
        /// <summary>
        /// Gets list of functions.
        /// </summary>
        /// <returns>List of functions.</returns>
        public IEnumerable<RMS_FunctionSel> RMS_FunctionSel()
        {
            var result = new List<RMS_FunctionSel>();

            using (var reader = ExecSpDataReader("RMS_FunctionSel"))
            {
                while (reader.Read())
                {
                    result.Add(new RMS_FunctionSel
                    {
                        FuncId = reader.ToInt32("funcid"),
                        FuncName = reader.ToString("funcname"),
                        FuncKey = reader.ToString("funckey"),
                        FuncDesc = reader.ToString("funcdesc"),
                        ParentId = reader.ToInt32("parentid"),
                        ParentName = reader.ToString("parentname"),
                        DisplayOrder = reader.ToInt32("displayorder"),
                        ModifiedDate = reader.ToDateTime("modifieddate")
                    });
                }
            }

            return result;
        }

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
        public Result RMS_FunctionIns(
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
            functionId = 0;
            var result = Result.Empty;

            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("funcname", name);
            param.Add("funckey", key);
            param.Add("funcdesc", description);
            param.Add("parentid", parentId);
            if (string.IsNullOrEmpty(parentName))
            {
                param.Add("parentname", DBNull.Value);
            }
            else
            {
                param.Add("parentname", parentName);
            }

            param.Add("displayorder", displayOrder);
            param.Add("ip", ip);

            var r = ExecSp("RMS_FunctionIns", param);
            result = r.ToResult();
            if (result.IsSuccessful)
            {
                functionId = DbConverter.ToInt32(r.Outputs["FuncID"]);
            }

            return result;
        }

        /// <summary>
        /// Updates a function information.
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
        public Result RMS_FunctionUpd(
                          int id, 
                          string name, 
                          string description, 
                          int parentId, 
                          string parentName, 
                          int displayOrder, 
                          int adminId, 
                          string ip)
        {
            Hashtable param = new Hashtable();
            param.Add("adminid", adminId);
            param.Add("funcid", id);
            param.Add("funcname", name);
            param.Add("funcdesc", description);
            param.Add("parentid", parentId);
            if (string.IsNullOrEmpty(parentName))
            {
                param.Add("parentname", DBNull.Value);
            }
            else
            {
                param.Add("parentname", parentName);
            }

            param.Add("displayorder", displayOrder);
            param.Add("ip", ip);

            var r = ExecSp("RMS_FunctionUpd", param);

            return r.ToResult();
        }
    }
}