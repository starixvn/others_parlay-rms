namespace ParlayRMS.DataAccess.Menus
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DTO;
    using ParlayRMS.DTO.Menus;
    using Utility;
    using Utility.Data;

    public class MenusDataAccess : BaseDataAccess
    {
        public IEnumerable<RMS_MenuSel> RMS_MenuSel()
        {
            var result = new List<RMS_MenuSel>();

            using (var reader = ExecSpDataReader("RMS_MenuSel"))
            {
                while (reader.Read())
                {
                    result.Add(new RMS_MenuSel
                    {
                        MenuId = reader.ToInt32("menuid"),
                        MenuName = reader.ToString("menuname"),
                        MenuDesc = reader.ToString("menudesc"),
                        Url = reader.ToString("url"),
                        ParentId = reader.ToInt32("parentid"),
                        ParentName = reader.ToString("parentname"),
                        FuncId = reader.ToInt32("funcid"),
                        FuncName = reader.ToString("funcname"),
                        DisplayOrder = reader.ToInt32("displayorder"),
                        ModifiedDate = reader.ToDateTime("ModifiedDate"),
                        Enabled = reader.ToBoolean("enabled")
                    });
                }
            }

            return result;
        }

        public Result RMS_MenuIns(
                          string name,
                          string description,
                          string url,
                          int functionId,
                          int parentId,
                          string parentName,
                          int displayOrder,
                          bool isActive,
                          int adminId,
                          string ip,
                          out int menuId)
        {
            menuId = 0;
            var result = Result.Empty;

            var param = new Hashtable
            {
                { "adminid", adminId },
                { "menuname", name },
                { "menudesc", description },
                { "url", url },
                { "funcid", functionId },
                { "parentid", parentId },
                { "parentname", parentName },
                { "displayorder", displayOrder },
                { "enabled", isActive },
                { "ip", ip }
            };

            var r = ExecSp("RMS_MenuIns", param);
            result = r.ToResult();
            if (result.IsSuccessful)
            {
                menuId = DbConverter.ToInt32(r.Outputs["menuid"]);
            }

            return result;
        }

        public Result RMS_MenuUpd(
                          int id,
                          string name,
                          string description,
                          string url,
                          int functionId,
                          int parentId,
                          string parentName,
                          int displayOrder,
                          int adminId,
                          string ip)
        {
            var param = new Hashtable
            {
                { "adminid", adminId },
                { "menuid", id },
                { "menuname", name },
                { "menudesc", description },
                { "url", url },
                { "funcid", functionId },
                { "parentid", parentId },
                { "parentname", parentName == null ? DBNull.Value : (object)parentName },
                { "displayorder", displayOrder },
                { "ip", ip }
            };

            var r = ExecSp("RMS_MenuUpd", param);

            return r.ToResult();
        }

        public Result RMS_MenuUpdStatus(int id, bool isActive, int adminId, string ip)
        {
            var param = new Hashtable
            {
                { "adminid", adminId },
                { "menuid", id },
                { "enabled", isActive },
                { "ip", ip }
            };

            var r = ExecSp("RMS_MenuUpdStatus", param);

            return r.ToResult();
        }
    }
}
