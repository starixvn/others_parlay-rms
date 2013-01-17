namespace ParlayRMS.DataAccess.Implement.Menus
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Caching;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DataAccess.Interface.Menus;
    using ParlayRMS.DTO.Menus;
    using ParlayRMS.Utility;
    using ParlayRMS.Utility.Data;
    using ParlayRMS.DTO;

    public class MenusDataAccess : BaseDataAccess, IMenusDataAccess
    {
        private static readonly ICache Cache = CacheService.CreateInstance(CacheType.MemoryCache);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RMS_MenuSel> RMS_MenuSel()
        {
            var result = new List<RMS_MenuSel>();

            try
            {
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
        /// <param name="description"></param>
        /// <param name="url"></param>
        /// <param name="functionId"></param>
        /// <param name="parentId"></param>
        /// <param name="parentName"></param>
        /// <param name="displayOrder"></param>
        /// <param name="isActive"></param>
        /// <param name="creator"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public Result RMS_MenuIns(string name, string description, string url, int functionId, int parentId, string parentName, int displayOrder, bool isActive, int adminId, string ip, out int menuId)
        {
            menuId = 0;
            var result = Result.Empty;

            try
            {
                Hashtable param = new Hashtable();
                param.Add("adminid", adminId);
                param.Add("menuname", name);
                param.Add("menudesc", description);
                param.Add("url", url);
                param.Add("funcid", functionId);
                param.Add("parentid", parentId);
                param.Add("parentname", parentName);
                param.Add("displayorder", displayOrder);
                param.Add("enabled", isActive);
                param.Add("ip", ip);

                var r = ExecSp("RMS_MenuIns", param);
                result = r.ToResult();
                if (result.IsSuccessful)
                {
                    menuId = DbConverter.ToInt32(r.Outputs["menuid"]);
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
        /// <param name="url"></param>
        /// <param name="functionId"></param>
        /// <param name="parentId"></param>
        /// <param name="parentName"></param>
        /// <param name="displayOrder"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public Result RMS_MenuUpd(int id, string name, string description, string url, int functionId, int parentId, string parentName, int displayOrder, int adminId, string ip)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("adminid", adminId);
                param.Add("menuid", id);
                param.Add("menuname", name);
                param.Add("menudesc", description);
                param.Add("url", url);
                param.Add("funcid", functionId);
                param.Add("parentid", parentId);
                param.Add("parentname", parentName == null ? DBNull.Value : (object)parentName);
                param.Add("displayorder", displayOrder);
                param.Add("ip", ip);

                var r = ExecSp("RMS_MenuUpd", param);
                return r.ToResult();
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return Result.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public Result RMS_MenuUpdStatus(int id, bool isActive, int adminId, string ip)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("adminid", adminId);
                param.Add("menuid", id);
                param.Add("enabled", isActive);
                param.Add("ip", ip);

                var r = ExecSp("RMS_MenuUpdStatus", param);
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