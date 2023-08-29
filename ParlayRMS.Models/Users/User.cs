namespace ParlayRMS.Models.Users
{
    using System;
    using DTO;
    using Groups;

    public class User : BaseEntityModel
    {
        public string PassWord { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public User Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public Group Group { get; set; }

        public Request Request { get; set; }
    }
}
