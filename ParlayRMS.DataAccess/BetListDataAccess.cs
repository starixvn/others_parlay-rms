namespace ParlayRMS.DataAccess
{
    using Fanex.Data.Repository;
    using Criteria.BetList;
    using DTO.BetList;

    public class BetListDataAccess 
    {
        private readonly IDynamicRepository repository = new DynamicRepository();

        public BetListData RMS_Parlay_TransDetails(long transId, long refNo)
        {
            var criteria = new ComboParlay_TransDetails_Criteria { TransId = transId, RefNo = refNo };
            var tickets = repository.Fetch<RMS_Parlay_TransDetails>(criteria);
            
            return new BetListData
            {
                CurrencyName = criteria.CurrencyName,
                Tickets = tickets
            };
        }

        public RMS_Monitor_Parlay_BetInfo RMS_Monitor_Parlay_BetInfo(long transId)
        {
            var criteria = new Monitor_ComboParlay_BetInfo_Criteria { TransId = transId };
            return repository.Get<RMS_Monitor_Parlay_BetInfo>(criteria);
        }
    }
}
