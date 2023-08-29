namespace ParlayRMS.DTO.Users
{
    using System;

    public class RMS_UserInfoSel
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Enabled { get; set; }
    }
}
