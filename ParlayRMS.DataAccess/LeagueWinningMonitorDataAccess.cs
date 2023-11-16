namespace ParlayRMS.DataAccess
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Criteria.LeagueWinningMonitor;
    using DTO.LeagueWinningMonitor;
    using Utility;
    using Utility.Data;
    using Fanex.Data.Repository;

    public class LeagueWinningMonitorDataAccess
    {
        private readonly IDynamicRepository repository = new DynamicRepository();


        public IEnumerable<RmsRptLeagueWinningMonitor> GetData(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal winningAmount,
            int selectTop,
            byte group,
            byte reportBy)
        {

            var criteria = new Rpt_League_Winning_Monitor_Criteria
            {
                Selection = combos,
                ParlayType = parlayTypes,
                SportType = sportsTypes,
                Level = levels,
                MaxPayout = winningAmount,
                LimitRowCount = selectTop,
                LeagueGroupId = group,
                ReportType = reportBy,
                LeagueId = 0,
                CustId = 0
            };
            return repository.Fetch<RmsRptLeagueWinningMonitor>(criteria);
        }

        public IEnumerable<RmsRptLeagueWinningMonitorDetail> GetDetailList(
            string combos,
            string parlayTypes,
            string sportsTypes,
            string levels,
            decimal winningAmount,
            int selectTop,
            byte group,
            byte reportBy,
            int leagueId,
            int custId)
        {
            var criteria = new Rpt_League_Winning_Monitor_Criteria
            {
                Selection = combos,
                ParlayType = parlayTypes,
                SportType = sportsTypes,
                Level = levels,
                MaxPayout = winningAmount,
                LimitRowCount = selectTop,
                LeagueGroupId = group,
                ReportType = reportBy,
                LeagueId = leagueId,
                CustId = custId
            };


            var detailList = new List<RmsRptLeagueWinningMonitorDetail>();
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        detailList.Add(new RmsRptLeagueWinningMonitorDetail
                        {
                            TransDate = reader.ToDateTime("transdate").ToString("MM/dd/yyyy h:mm:ss tt"),
                            TransId = reader.ToInt64("transid"),
                            Username = reader.ToString("Username"),
                            Level = reader["level"] == DBNull.Value ? "None" : reader.ToByte("level").ToString(),
                            Odds = reader.ToDecimal("odds"),
                            Stake = reader.ToDecimal("stake"),
                            WinningAmount = reader.ToDecimal("winningAmt"),
                            CompanyLoseAmount = reader.ToDecimal("companyLoseAmt"),
                            Combo = reader.ToInt32("combo"),
                            TotalSelection = Convert.ToInt32(reader.ToDecimal("totalSelection")),
                            SportType = reader.ToInt32("Sporttype"),
                            SportTypeName = CacheRef.GetSportNameDropDown(reader.ToInt32("Sporttype")),
                            IP = reader.ToString("IP"),
                            CustId = reader.ToInt32("custid"),
                            BetType = reader.ToInt32("bettype"),
                            RefNo = reader.ToInt64("refno"),
                            SiteGroup = reader.ToString("SiteGroup"),
                            IsParlayGenerator = reader.ToBoolean("IsParlayGenerator")
						});
                    }
                }
            }

            return detailList;
        }
    }
}
