// -----------------------------------------------------------------------
// <copyright file="UpdateStatusModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.ManageUser
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateStatusModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}