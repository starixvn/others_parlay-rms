namespace ParlayRMS.ViewModel.ManageMember
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UpdateStatusModel
    {
        public int custId { get; set; }

        public bool following { get; set; }
    }
}