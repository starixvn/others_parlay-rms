namespace ParlayRMS.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Criteria.TotalOutstanding;
    using DTO.TotalOutstanding;

    public class TotalOutstandingDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();
        public List<RMS_Rpt_Parlay_Outstanding_Mem> RMS_Rpt_Parlay_Outstanding_Mem(int custId, string combo, string sportsType, string parlaysType, bool isCornerParlay)
        {
            var criteria = new Rpt_ComboParlay_Outstanding_Mem_Criteria
            {
                CustId = custId,
                Selection = combo,
                SportType = sportsType,
                ParlayType = parlaysType,
                IsCornerParlay = isCornerParlay
            };
            var list = repository.Fetch<RMS_Rpt_Parlay_Outstanding_Mem>(criteria).ToList();
            list.ForEach(l =>
            {
                l.CustomerClass=string.IsNullOrWhiteSpace(l.CustomerClass)?"None":l.CustomerClass;
            });

            return list;
        }

        public List<RMS_Rpt_Parlay_Outstanding_Ag> RMS_Rpt_Parlay_Outstanding_Ag(int custId, string combo, string sportsType, string parlaysType, bool isCornerParlay)
        {
            var criteria = new Rpt_ComboParlay_Outstanding_Ag_Criteria
            {
                CustId = custId,
                Selection = combo,
                SportType = sportsType,
                ParlayType = parlaysType,
                IsCornerParlay = isCornerParlay
            };
            var list = repository.Fetch<RMS_Rpt_Parlay_Outstanding_Ag>(criteria).ToList();
            list.ForEach(l =>
            {
                l.CustomerClass = string.IsNullOrWhiteSpace(l.CustomerClass) ? "None" : l.CustomerClass;
            });

            return list;
        }
    }
}
