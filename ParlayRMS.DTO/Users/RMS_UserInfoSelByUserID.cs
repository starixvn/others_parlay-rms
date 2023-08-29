namespace ParlayRMS.DTO.Users
{
    public class RMS_UserInfoSelByUserID
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public bool Enabled { get; set; }
    }
}
