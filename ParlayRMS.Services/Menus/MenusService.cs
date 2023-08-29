namespace ParlayRMS.Services.Menus
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.Menus;
    using Models.Functions;
    using DTO;
    using ParlayRMS.Models.Menus;
    using Systems;

    public class MenusService : BaseService
    {
        private readonly MenusDataAccess _menuDataAccess = new MenusDataAccess();

        public IEnumerable<Menu> GetAllMenus()
        {
            var data = _menuDataAccess.RMS_MenuSel();
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

        public Result CreateMenu(
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
            return _menuDataAccess.RMS_MenuIns(name, description, url, functionId, parentId, parentName, displayOrder, isActive, adminId, ip, out menuId);
        }

        public Result UpdateMenu(
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
            return _menuDataAccess.RMS_MenuUpd(id, name, description, url, functionId, parentId, parentName, displayOrder, adminId, ip);
        }

        public Result UpdateMenuStatus(int id, bool isActive, int adminId, string ip)
        {
            return _menuDataAccess.RMS_MenuUpdStatus(id, isActive, adminId, ip);
        }
    }
}
