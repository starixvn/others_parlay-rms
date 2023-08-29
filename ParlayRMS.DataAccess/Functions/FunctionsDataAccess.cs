namespace ParlayRMS.DataAccess.Functions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DTO;
    using ParlayRMS.DTO.Functions;
    using Utility;
    using Utility.Data;

    public class FunctionsDataAccess : BaseDataAccess
    {
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

            var param = new Hashtable
            {
                { "adminid", adminId },
                { "funcname", name },
                { "funckey", key },
                { "funcdesc", description },
                { "parentid", parentId }
            };
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
            var param = new Hashtable
            {
                { "adminid", adminId },
                { "funcid", id },
                { "funcname", name },
                { "funcdesc", description },
                { "parentid", parentId }
            };
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
