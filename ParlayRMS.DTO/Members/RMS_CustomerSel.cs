// --------------------------------------------------------------------------
// <copyright file="RMS_CustomerSel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO.Members
{
    /// <summary>
    /// Customer model.
    /// </summary>
    public class RMS_CustomerSel
    {
        public int No { get; set; }

        public string Site { get; set; }

        public int Custid { get; set; }

        public int Level { get; set; }

        public string Username { get; set; }
    }
}