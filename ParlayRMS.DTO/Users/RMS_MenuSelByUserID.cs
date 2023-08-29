namespace ParlayRMS.DTO.Users
{
    using System.Collections.Generic;

    public class RMS_MenuSelByUserID
    {
        public IEnumerable<RMS_MenuSelByUserID_Menu> Menus { get; set; }

        public IEnumerable<RMS_MenuSelByUserID_Function> Functions { get; set; }
    }
}
