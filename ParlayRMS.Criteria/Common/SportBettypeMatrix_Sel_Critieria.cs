using Fanex.Data.Repository;

namespace ParlayRMS.Criteria.Common
{
    public class SportBettypeMatrix_Sel_Critieria : CriteriaBase
    {
        public int Mode { get; set; }

        public string SubProduct { get; set; }

        public override string GetSettingKey() => "Acc_SportBettypeMatrix_Sel";

        public override bool IsValid() => true;
    }
}