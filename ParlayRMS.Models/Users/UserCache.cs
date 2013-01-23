namespace ParlayRMS.Models.Users
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Menus;

    public class UserCache
    {
        public DateTime CreatedTime { get; private set; }

        public bool IsExpired
        {
            get
            {
                if (this.CreatedTime.AddMinutes(1) < DateTime.Now)
                {
                    return true;
                }

                return false;
            }
        }

        public IEnumerable<Menu> Menus { get; private set; }

        public IEnumerable<FunctionPolicy> Functions { get; private set; }

        public UserCache(IEnumerable<Menu> menus, IEnumerable<FunctionPolicy> functions)
        {
            CreatedTime = DateTime.Now;
            this.Menus = menus;
            this.Functions = functions;
        }
    }
}