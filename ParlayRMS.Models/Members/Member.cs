namespace ParlayRMS.Models.Members
{
    using System;
    using Users;

    public class Member
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public MemberLevel Level { get; set; }

        public int Following { get; set; }

        public DateTime CreateDate { get; set; }

        public User Creator { get; set; }
    }
}
