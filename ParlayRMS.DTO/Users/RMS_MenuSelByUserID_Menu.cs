// -----------------------------------------------------------------------
// <copyright file="RMS_MenuSelByUserID_Menu.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.Users
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RMS_MenuSelByUserID_Menu
    {
        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public int MenuId { get; set; }

        public string MenuName { get; set; }

        public string Url { get; set; }

        public int DisplayOrder { get; set; }
    }
}