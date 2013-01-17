namespace ParlayRMS.Models.Users
{
    using global::System.Collections.Generic;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Menus;

    public class UserCache
    {
        public IEnumerable<Menu> Menus { get; set; }

        public IEnumerable<FunctionPolicy> Functions { get; set; }
    }
}