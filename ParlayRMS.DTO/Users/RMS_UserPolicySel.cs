namespace ParlayRMS.DTO.Users
{
    public class RMS_UserPolicySel
    {
        public int FuncId { get; set; }

        public string FuncKey { get; set; }

        public string FuncName { get; set; }

        public int DisplayOrder { get; set; }

        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public int UserPolicy { get; set; }

        public int GroupPolicy { get; set; }
    }
}
