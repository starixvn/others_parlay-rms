// -----------------------------------------------------------------------
// <copyright file="RMS_MenuSelByUserID.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.Users
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RMS_MenuSelByUserID
    {
        public IEnumerable<RMS_MenuSelByUserID_Menu> Menus { get; set; }

        public IEnumerable<RMS_MenuSelByUserID_Function> Functions { get; set; }
    }
}