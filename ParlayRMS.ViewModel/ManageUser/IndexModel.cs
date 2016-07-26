// --------------------------------------------------------------------------
// <copyright file="IndexModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageUser
{
    using System.Web.Mvc;

    /// <summary>
    /// Index Model.
    /// </summary>
    public class IndexModel
    {
        [AllowHtml]
        public string UserName { get; set; }

        public int GroupId { get; set; }
    }
}