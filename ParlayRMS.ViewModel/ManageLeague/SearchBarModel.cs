namespace ParlayRMS.ViewModel.ManageLeague
{
    using System.ComponentModel.DataAnnotations;
    using Utility.Data;

    public class SearchBarModel
    {
        private int _group;
        public SearchBarModel()
        {
            Group = 0;
        }

        public string League { get; set; }

        [DisplayFormat(NullDisplayText = "")]
        public int Group
        {
            get => _group;

            set
            {
                _group = value;

                Color = ModelsHelper.GetColorByLeagueGroup(Group);
                GroupName = ModelsHelper.GetNameByLeagueGroup(Group);
            }
        }

        public string Color { get; private set; }

        public string GroupName { get; private set; }
    }
}
