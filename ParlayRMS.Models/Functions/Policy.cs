// --------------------------------------------------------------------------
// <copyright file="Policy.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.Models
{
    /// <summary>
    /// Policy Model.
    /// </summary>
    public class Policy
    {
        private int _permission = 0;

        public Policy(int p)
        {
            this._permission = p;
        }

        public bool Writable
        {
            get
            {
                if (this._permission == 2)
                {
                    return true;
                }

                return false;
            }
        }

        public bool Readable
        {
            get
            {
                if (this._permission == 1 || this._permission == 2)
                {
                    return true;
                }

                return false;
            }
        }
    }
}