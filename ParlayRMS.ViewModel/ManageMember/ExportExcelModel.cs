// -----------------------------------------------------------------------
// <copyright file="ExportExcelModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExportExcelModel
    {
        [Required]
        public string UserName { get; set; }

        [Required, Range(0, 10)]
        public int Level { get; set; }
    }
}