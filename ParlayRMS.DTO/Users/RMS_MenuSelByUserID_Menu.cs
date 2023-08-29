namespace ParlayRMS.DTO.Users
{
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
