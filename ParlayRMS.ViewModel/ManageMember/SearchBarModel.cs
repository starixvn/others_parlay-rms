// --------------------------------------------------------------------------
// <copyright file="SearchBarModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;
    using ParlayRMS.Utility;
    using Utility.Data;

    /// <summary>
    /// Search Bar Model.
    /// </summary>
    public class SearchBarModel
    {
        private int? _level;

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
                return this._level;
            }

            set
            {
                if (this._level != value)
                {
                    this._level = value;
                }

                var cl = "alllevel";
                if (this.Level != null)
                {
                    cl = ModelsHelper.GetClassByMemberLevel(this.Level.Value);
                }

                this.ClassName = cl;

                var levelName = ViewLang.AllLevel;
                if (this.Level != null)
                {
                    if (this.Level.Value == 0)
                    {
                        levelName = ViewLang.None;
                    }
                    else
                    {
                        levelName = "Level " + this.Level.Value;
                    }
                }

                this.LevelName = levelName;
            }
        }

        public string ClassName { get; private set; }

        public string LevelName { get; private set; }
    }
}