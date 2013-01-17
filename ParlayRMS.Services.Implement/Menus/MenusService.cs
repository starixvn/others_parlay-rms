namespace ParlayRMS.Services.Implement.Menus
{
    using System.Linq;
    using System.Collections.Generic;
    using ParlayRMS.DataAccess.Implement.Menus;
    using ParlayRMS.DataAccess.Interface.Menus;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Interface;
    using ParlayRMS.Services.Interface.Menus;
    using ParlayRMS.Models.Menus;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO;
    using ParlayRMS.Services.Implement.Systems;

    public class MenusService : BaseService, IMenusService
    {
        private readonly IMenusDataAccess menuDataAccess = new MenusDataAccess();

        public IEnumerable<Menu> GetAllMenus()
        {
            var data = this.menuDataAccess.RMS_MenuSel();
            return data.Select(x => new Menu
            {
                Id = x.MenuId,
                Name = x.MenuName,
                Description = x.MenuDesc,
                Url = x.Url,
                Parent = x.ParentId == 0 ? null : new Menu
                {
                    Id = x.ParentId,
                    Name = x.ParentName
                },
                Function = new Function
                {
                    Id = x.FuncId,
                    Name = x.FuncName
                },
                DisplayOrder = x.DisplayOrder,
                IsActive = x.Enabled,
                ModifiedDate = x.ModifiedDate
            });
        }

        public IEnumerable<Menu> GetMenusByUserId(int userId)
        {
            var cachingService = new CachingService();
            var userCache = cachingService.GetUserCache(userId);
            return userCache.Menus;
        }

        public Result CreateMenu(string name, string description, string url, int functionId, int parentId, string parentName, int displayOrder, bool isActive, int adminId, string ip, out int menuId)
        {
            return this.menuDataAccess.RMS_MenuIns(name, description, url, functionId, parentId, parentName, displayOrder, isActive, adminId, ip, out menuId);
        }

        public Result UpdateMenu(int id, string name, string description, string url, int functionId, int parentId, string parentName, int displayOrder, int adminId, string ip)
        {
            Result result = this.menuDataAccess.RMS_MenuUpd(id, name, description, url, functionId, parentId, parentName, displayOrder, adminId, ip);
            if (result.IsSuccessful)
            {
                this.OnMenuUpdated();
            }

            return result;
        }

        public Result UpdateMenuStatus(int id, bool isActive, int adminId, string ip)
        {
            Result result = this.menuDataAccess.RMS_MenuUpdStatus(id, isActive, adminId, ip);
            if (result.IsSuccessful)
            {
                this.OnMenuUpdated();
            }

            return result;
        }

        private void OnMenuUpdated()
        {
            var cachingService = new CachingService();
            cachingService.ClearUserCache();
        }
    }
}