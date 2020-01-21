namespace ParlayRMS.ViewModel.ManageLeague
{
    using ParlayRMS.Common.Models;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Search Bar Model.
    /// </summary>
    public class SearchBarModel
    {
        private int _group;
        public SearchBarModel()
        {
            this.Group = 0;
        }

        public string League { get; set; }

        [DisplayFormat(NullDisplayText = "")]
        public int Group
        {
            get
            {
                return this._group;
            }

            set
            {
                if (this._group != value)
                {
                    this._group = value;
                }

                this.Color = ModelsHelper.GetColorByLeagueGroup(this.Group);
                this.GroupName = ModelsHelper.GetNameByLeagueGroup(this.Group);
            }
        }

        public string Color { get; private set; }

        public string GroupName { get; private set; }
    }
}