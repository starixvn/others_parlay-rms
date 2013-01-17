namespace ParlayRMS.DataAccess.Implement.Functions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DataAccess.Interface.Functions;
    using ParlayRMS.DTO.Functions;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;
    using ParlayRMS.DTO;

    public class FunctionsDataAccess : BaseDataAccess, IFunctionsDataAccess
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RMS_FunctionSel> RMS_FunctionSel()
        {
            var result = new List<RMS_FunctionSel>();

            try
            {
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
        /// <param name="name"></param>
        /// <param name="key"></param>
        /// <param name="description"></param>
        /// <param name="parentId"></param>
        /// <param name="parentName"></param>
        /// <param name="displayOrder"></param>
        /// <param name="creator"></param>
        /// <param name="functionId"></param>
        /// <returns></returns>
        public Result RMS_FunctionIns(string name, string key, string description, int parentId, string parentName, int displayOrder, int adminId, string ip, out int functionId)
        {
            functionId = 0;
            var result = Result.Empty;

            try
            {
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
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="parentId"></param>
        /// <param name="parentName"></param>
        /// <param name="displayOrder"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public Result RMS_FunctionUpd(int id, string name, string description, int parentId, string parentName, int displayOrder, int adminId, string ip)
        {
            try
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
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return Result.Empty;
        }
    }
}