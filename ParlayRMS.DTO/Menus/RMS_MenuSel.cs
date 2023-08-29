using System;

namespace ParlayRMS.DTO.Menus
{
    public class RMS_MenuSel
    {
        public int MenuId { get; set; }

        public string MenuName { get; set; }

        public string MenuDesc { get; set; }

        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public string Url { get; set; }

        public int FuncId { get; set; }

        public string FuncName { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime ModifiedDate { get; set; }

        public bool Enabled { get; set; }
    }
}
