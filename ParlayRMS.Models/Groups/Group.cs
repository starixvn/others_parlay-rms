namespace ParlayRMS.Models.Groups
{
    using System;
    using Users;

    public class Group : BaseEntityModel
    {
        public string Description { get; set; }

        public int Policy { get; set; }

        public User Creator { get; set; }

        public DateTime CreateDate { get; set; }

        public int NumberOfMember { get; set; }
    }
}
