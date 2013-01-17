namespace ParlayRMS.ViewModel.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MenuItem
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public List<MenuItem> Children { get; set; }
    }
}