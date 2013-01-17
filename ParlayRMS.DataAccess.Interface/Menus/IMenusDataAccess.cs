namespace ParlayRMS.DataAccess.Interface.Menus
{
    using System.Collections.Generic;
    using ParlayRMS.Common.Models;
    using ParlayRMS.DTO.Menus;
    using ParlayRMS.DTO;

    public interface IMenusDataAccess
    {
        IEnumerable<RMS_MenuSel> RMS_MenuSel();

        Result RMS_MenuIns(string name, string description, string url, int functionId, int parentId, string parentName, int displayOrder, bool isActive, int adminId, string ip, out int menuId);

        Result RMS_MenuUpd(int id, string name, string description, string url, int functionId, int parentId, string parentName, int displayOrder, int adminId, string ip);

        Result RMS_MenuUpdStatus(int id, bool isActive, int adminId, string ip);
    }
}