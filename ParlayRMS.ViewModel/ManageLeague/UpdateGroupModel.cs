namespace ParlayRMS.ViewModel.ManageLeague
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateGroupModel
    {
        [Required]
        public int League { get; set; }

        [Required]
        public int Group { get; set; }
    }
}