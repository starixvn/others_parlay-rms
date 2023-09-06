namespace ParlayRMS.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Criteria.Common;
    using DTO.Common;

    public class CommonDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public RMS_SelBalanceUpDay RMS_SelBalanceUpDay()
        {
            return repository.Get<RMS_SelBalanceUpDay>(new SelBalanceUpDay_Criteria());
        }

        public RMS_Rpt_Parlay_SelMinTransdate RMS_Rpt_Parlay_SelMinTransdate()
        {
            return repository.Get<RMS_Rpt_Parlay_SelMinTransdate>(new Rpt_Parlay_SelMinTransdate_Criteria());
        }

        public List<int> GetBettypeBelongsBetRadar()
        {
            var criteria = new SportBettypeMatrix_Sel_Critieria
            {
                Mode = 4,
                SubProduct = "BetRadar"
            };
            var list = repository.Fetch<Acc_SportBettypeMatrix_Sel>(criteria);
            
            return list.Select(s =>s.BetType).ToList();
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            var criteria = new CurrencySel_Criteria();
            return repository.Fetch<Currency>(criteria);
        }

        public IEnumerable<Company> RMS_SiteByGroupSel()
        {
            var criteria = new SiteByGroupSel_Criteria();
            return repository.Fetch<Company>(criteria);
        }
    }
}
