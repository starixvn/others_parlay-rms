namespace ParlayRMS.DataAccess.Implement.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DataAccess.Interface.BetList;
    using ParlayRMS.DTO.BetList;
    using ParlayRMS.Utility.Data;

    public class BetListDataAccess : BaseDataAccess, IBetListDataAccess
    {
        public IEnumerable<RMS_Parlay_TransDetails> RMS_Parlay_TransDetails(long refNo)
        {
            var result = new List<RMS_Parlay_TransDetails>();

            try
            {
                using (var db = new ObjectDb("RMS_Parlay_TransDetails"))
                {
                    db.AddInParameter("refno", DbType.Int64, refNo);
                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new RMS_Parlay_TransDetails
                            {
                                TransId = reader.ToInt64("transid"),
                                BetType = reader.ToInt32("bettype"),
                                HomeId = reader.ToInt32("homeid"),
                                AwayId = reader.ToInt32("awayid"),
                                Hdp1 = reader.ToDecimal("hdp1"),
                                Hdp2 = reader.ToDecimal("hdp2"),
                                BetTeam = reader.ToString("betteam"),
                                Odds = reader.ToDecimal("odds"),
                                StatusId = reader.ToInt32("statusid"),
                                Status = reader.ToString("status"),
                                LiveHomeScore = reader.ToInt32("livehomescore"),
                                LiveAwayScore = reader.ToInt32("liveawayscore"),
                                LiveIndicator = reader.ToBoolean("liveindicator"),
                                LeagueId = reader.ToInt32("leagueid"),
                                SportType = reader.ToInt32("sporttype"),
                                IsNeutral = reader.ToBoolean("IsNeutral"),
                                MatchId = reader.ToInt32("matchid"),
                                MatchCode = reader.ToInt32("matchcode"),
                                BetStatus = reader.ToString("betstatus")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return result;
        }
    }
}