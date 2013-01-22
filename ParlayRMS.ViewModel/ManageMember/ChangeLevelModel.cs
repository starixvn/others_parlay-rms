// -----------------------------------------------------------------------
// <copyright file="ChangeLevelModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.ManageMember
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChangeLevelModel
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required, Range(0, 10)]
        public int Level { get; set; }
    }
}
