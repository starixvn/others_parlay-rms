namespace ParlayRMS.ViewModel.ManageMember
{
    using System.ComponentModel.DataAnnotations;
    using Utility;
    using Utility.Data;

    public class SearchBarModel
    {
        private int? _level;

        public SearchBarModel()
        {
            ClassName = "alllevel";
            LevelName = ViewLang.AllLevel;
        }

        public string UserName { get; set; }

        [DisplayFormat(NullDisplayText = "")]
        public int? Level
        {
            get => _level;

            set
            {
                _level = value;

                var cl = "alllevel";
                if (Level != null)
                {
                    cl = ModelsHelper.GetClassByMemberLevel(Level.Value);
                }

                ClassName = cl;

                var levelName = ViewLang.AllLevel;
                if (Level != null)
                {
                    if (Level.Value == 0)
                    {
                        levelName = ViewLang.None;
                    }
                    else
                    {
                        levelName = "Level " + Level.Value;
                    }
                }

                LevelName = levelName;
            }
        }

        public string ClassName { get; private set; }

        public string LevelName { get; private set; }
    }
}
