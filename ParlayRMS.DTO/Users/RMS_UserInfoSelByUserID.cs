// -----------------------------------------------------------------------
// <copyright file="RMS_UserInfoSelByUserID.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.Users
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
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