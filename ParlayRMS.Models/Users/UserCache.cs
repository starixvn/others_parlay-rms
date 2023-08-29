namespace ParlayRMS.Models.Users
{
    using System;
    using System.Collections.Generic;
    using Functions;
    using Menus;

    public class UserCache
    {
        private const int UserPolicyCacheTime = 5;
        public UserCache(IEnumerable<Menu> menus, IEnumerable<FunctionPolicy> functions)
        {
            CreatedTime = DateTime.Now;
            Menus = menus;
            Functions = functions;
        }

        public DateTime CreatedTime { get; private set; }

        public bool IsExpired
        {
            get
            {
                if (CreatedTime.AddSeconds(UserPolicyCacheTime) < DateTime.Now)
                {
                    return true;
                }

                return false;
            }
        }

        public IEnumerable<Menu> Menus { get; private set; }

        public IEnumerable<FunctionPolicy> Functions { get; private set; }
    }
}
