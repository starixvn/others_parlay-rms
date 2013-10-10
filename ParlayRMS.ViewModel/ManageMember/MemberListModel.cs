// --------------------------------------------------------------------------
// <copyright file="MemberListModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageMember
{
    using System.Web.Mvc;

    /// <summary>
    /// Member List Model.
    /// </summary>
    public class MemberListModel
    {
        [AllowHtml]
        public string UserName { get; set; }

        public int? Level { get; set; }

        public int? Page { get; set; }
    }
}