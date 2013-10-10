// --------------------------------------------------------------------------
// <copyright file="Request.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO
{
    /// <summary>
    /// Request model.
    /// </summary>
    public class Request
    {
        public string Ip { get; set; }

        public string Domain { get; set; }

        public string Browser { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public string Language
        {
            get
            {
                return "en-US";
            }
        }
    }
}