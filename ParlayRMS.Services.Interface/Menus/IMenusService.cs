namespace ParlayRMS.Services.Interface.Menus
{
    using System.Collections.Generic;
    using ParlayRMS.Models;
    using ParlayRMS.Models.Menus;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO;

    public interface IMenusService
    {
        IEnumerable<Menu> GetAllMenus();

        IEnumerable<Menu> GetMenusByUserId(int userId);

        Result CreateMenu(string name, string description, string url, int functionId, int parentId, string parentName, int displayOrder, bool isActive, int adminId, string ip, out int menuId);

        Result UpdateMenu(int id, string name, string description, string url, int functionId, int parentId, string parentName, int displayOrder, int adminId, string ip);

        Result UpdateMenuStatus(int id, bool isActive, int adminId, string ip);
    }
}