namespace ParlayRMS.DataAccess.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using ParlayRMS.DTO.Common;
    using Utility.Data;

    public class CommonDataAccess : BaseDataAccess
    {
        #region Balance Up Day

        public RMS_SelBalanceUpDay RMS_SelBalanceUpDay()
        {
            using (var db = new ObjectDb("RMS_SelBalanceUpDay"))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new RMS_SelBalanceUpDay
                        {
                            BalanceUpDay = reader.ToDateTime("BalanceUpDay"),
                            MoveBetsDay = reader.ToDateTime("MoveBetsDay")
                        };
                    }
                }
            }

            return null;
        }

        #endregion Balance Up Day

        public RMS_Rpt_Parlay_SelMinTransdate RMS_Rpt_Parlay_SelMinTransdate()
        {
            using (var db = new ObjectDb("RMS_Rpt_Parlay_SelMinTransdate"))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new RMS_Rpt_Parlay_SelMinTransdate
                        {
                            MinTransdate = reader.ToDateTime("transdate_min")
                        };
                    }
                }
            }

            return null;
        }

        public List<int> GetBettypeBelongsBetRadar()
        {
            var bettypes = new List<int>();
            using (var db = new ObjectDb("Acc_SportBettypeMatrix_Sel"))
            {
                db.AddInParameter("mode", DbType.Int16, 4);
                db.AddInParameter("subproduct", DbType.String, "BetRadar");

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bettypes.Add(reader.ToInt32("bettype"));
                    }
                }
            }

            return bettypes;
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            var currencies = new List<Currency>();
            using (var db = new ObjectDb("RMS_CurrencySel"))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currencies.Add(new Currency { Id = reader.ToInt32("currency"), CurrencyCode = reader.ToString("currencyName") });
                    }
                }
            }

            return currencies;
        }

        public IEnumerable<Company> RMS_SiteByGroupSel()
        {
            var result = new List<Company>();

            using (var db = new ObjectDb("RMS_SiteByGroupSel"))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Company
                        {
                            Site = reader.ToString("Site"),
                            IsDeposit = reader.ToInt16("isDeposit"),
                        });
                    }
                }
            }

            return result;
        }
    }
}
