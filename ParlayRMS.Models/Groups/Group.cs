using System;
using ParlayRMS.Models.Users;

namespace ParlayRMS.Models.Groups
{
    public class Group : BaseEntityModel
    {
        public string Description { get; set; }

        public int Policy { get; set; }

        public User Creator { get; set; }

        public DateTime CreateDate { get; set; }

        public int NumberOfMember { get; set; }
    }
}