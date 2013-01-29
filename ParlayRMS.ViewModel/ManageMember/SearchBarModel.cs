// -----------------------------------------------------------------------
// <copyright file="SearchBarModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Common.Models;
    using ParlayRMS.Utility;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SearchBarModel
    {
        public SearchBarModel()
        {
            this.ClassName = "alllevel";
            this.LevelName = ViewLang.AllLevel;
        }

        public string UserName { get; set; }

        [DisplayFormat(NullDisplayText = "")]
        public int? Level
        {
            get
            {
                return this.level;
            }
            set
            {
                if (this.level != value)
                {
                    this.level = value;
                }

                var cl = "alllevel";
                if (this.Level != null)
                {
                    cl = ModelsHelper.GetClassByMemberLevel(this.Level.Value);
                }

                this.ClassName = cl;

                var lvName = ViewLang.AllLevel;
                if (this.Level != null)
                {
                    if (this.Level.Value == 0)
                    {
                        lvName = ViewLang.None;
                    }
                    else
                    {
                        lvName = "Level " + this.Level.Value;
                    }
                }

                this.LevelName = lvName;
            }
        }

        public string ClassName { get; private set; }

        public string LevelName { get; private set; }

        private int? level;
    }
}