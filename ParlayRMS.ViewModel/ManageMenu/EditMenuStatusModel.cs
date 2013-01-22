namespace ParlayRMS.ViewModel.ManageMenu
{
    using System.ComponentModel.DataAnnotations;

    public class EditMenuStatusModel
    {
        [Required]
        public int MenuId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}