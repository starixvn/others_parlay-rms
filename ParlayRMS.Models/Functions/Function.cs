namespace ParlayRMS.Models.Functions
{
    using System;
    using Users;

    public class Function : BaseEntityModel
    {
        public string Description { get; set; }

        public string Key { get; set; }

        public Function Parent { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public User Creator { get; set; }

        public int DisplayOrder { get; set; }

        public Policy GroupPolicy { get; set; }

        public Policy UserPolicy { get; set; }
    }
}
