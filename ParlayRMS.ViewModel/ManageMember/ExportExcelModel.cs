namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;

    public class ExportExcelModel
    {
        [Required]
        public string UserName { get; set; }

        [Range(0, 10)]
        public int? Level { get; set; }
    }
}
