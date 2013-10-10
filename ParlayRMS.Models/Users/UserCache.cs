// --------------------------------------------------------------------------
// <copyright file="UserCache.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models.Users
{
    using System;
    using System.Collections.Generic;
    using ParlayRMS.Models.Functions;
    using ParlayRMS.Models.Menus;
    using ParlayRMS.Utility;

    /// <summary>
    /// User cache.
    /// </summary>
    public class UserCache
    {        
        public UserCache(IEnumerable<Menu> menus, IEnumerable<FunctionPolicy> functions)
        {
            CreatedTime = DateTime.Now;
            this.Menus = menus;
            this.Functions = functions;
        }

        public DateTime CreatedTime { get; private set; }

        public bool IsExpired
        {
            get
            {
                if (this.CreatedTime.AddSeconds(AppSetting.UserPolicyCacheTime) < DateTime.Now)
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