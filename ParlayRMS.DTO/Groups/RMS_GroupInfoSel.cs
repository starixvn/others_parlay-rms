// -----------------------------------------------------------------------
// <copyright file="RMS_GroupInfoSel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.DTO.Groups
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RMS_GroupInfoSel
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int NoUsers { get; set; }

        public string Description { get; set; }

        public string Creator { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}