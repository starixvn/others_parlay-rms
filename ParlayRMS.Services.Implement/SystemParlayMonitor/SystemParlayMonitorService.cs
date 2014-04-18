// --------------------------------------------------------------------------
// <copyright file="SystemParlayMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.1.21">Created</change>
// </history>

namespace ParlayRMS.Services.Implement.SystemParlayMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ParlayRMS.DataAccess.Implement.SystemParlayMonitor;
    using ParlayRMS.DataAccess.Interface.SystemParlayMonitor;
    using ParlayRMS.DTO.SystemParlayMonitor;
    using ParlayRMS.Models;
    using ParlayRMS.Services.Interface.SystemParlayMonitor;
    using ParlayRMS.Utility;

    /// <summary>
    /// System parlay monitor service.
    /// </summary>
    public class SystemParlayMonitorService : BaseService, ISystemParlayMonitorService
    {
        private readonly ISystemParlayMonitorDataAccess _systemparlayDataAccess = new SystemParlayMonitorDataAccess();

        /// <summary>
        /// Gets data for system parlay monitor.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="sportType">Type of the sports.</param>
        /// <param name="level">The level.</param>
        /// <param name="stake">The stake.</param>
        /// <param name="maxTransId">The maximum trans identifier.</param>
        /// <param name="limitRowCount">The limit row count.</param>
        /// <returns>Data of system parlay monitor.</returns>
        public IEnumerable<RMS_Monitor_SystemParlay_Betlist> GetSystemParlayMonitorData(
                                                          string selection,
                                                          string sportType,
                                                          string level,
                                                          decimal stake,
                                                          long maxTransId,
                                                          int limitRowCount)
        {
            var data = _systemparlayDataAccess.RMS_Monitor_SystemParlay_Betlist(selection, sportType, level, stake, maxTransId, limitRowCount);

            ////if (!data.Any())
            ////{
            ////    data = CreateTestData(selection, level, stake, maxTransId);
            ////}

            foreach (var item in data)
            {
                item.Odds = Math.Round(CalculateOdds(item), 2);
                item.Stake = Math.Round(item.Stake, 2);
                item.NetWinAfterPT = Math.Round(CalculateNetWin(item), 2);
                item.ComboName = GetComboName(item.Combo);
            }

            return data;
        }

        /// <summary>
        /// Calculates the odds of a ticket.
        /// </summary>
        /// <param name="item">The ticket's data.</param>
        /// <returns>Actual odds of the ticket.</returns>
        private decimal CalculateOdds(RMS_Monitor_SystemParlay_Betlist item)
        {
            decimal result = 0;
            decimal sumOfCombination = 0;
            IEnumerable<List<decimal>> combinations = MathHelper.GetCombinations<decimal>(item.SubOdds).Where(obj => obj.Count > 1);

            foreach (List<decimal> combination in combinations)
            {
                sumOfCombination = 1;

                foreach (decimal odds in combination)
                {
                    sumOfCombination *= odds;
                }

                result += sumOfCombination;
            }

            return result;
        }

        /// <summary>
        /// Calculates the net win after PT of a ticket.
        /// </summary>
        /// <param name="item">The ticket's data.</param>
        /// <returns>Net max payout after PT of the ticket.</returns>
        private decimal CalculateNetWin(RMS_Monitor_SystemParlay_Betlist item)
        {
            decimal result = item.Odds * item.Stake;
            int numberOfBets = GetSystemParlayBets(item.Combo);

            result = result - (item.Stake * numberOfBets);

            return result;
        }

        /// <summary>
        /// Gets number of bets of the system parlay.
        /// </summary>
        /// <param name="combo">The combo.</param>
        /// <returns>Number of bets.</returns>
        private int GetSystemParlayBets(int combo)
        {
            SystemParlayBets result = 0;

            switch ((SystemParlayCombo)combo)
            {
                case SystemParlayCombo.Trixie:
                    result = SystemParlayBets.Trixie;
                    break;

                case SystemParlayCombo.Yankee:
                    result = SystemParlayBets.Yankee;
                    break;

                case SystemParlayCombo.Canadian:
                    result = SystemParlayBets.Canadian;
                    break;

                case SystemParlayCombo.Heinz:
                    result = SystemParlayBets.Heinz;
                    break;

                case SystemParlayCombo.SuperHeinz:
                    result = SystemParlayBets.SuperHeinz;
                    break;

                case SystemParlayCombo.Goliath:
                    result = SystemParlayBets.Goliath;
                    break;
            }

            return (int)result;
        }

        /// <summary>
        /// Creates the test data.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="level">The level.</param>
        /// <param name="stake">The stake.</param>
        /// <param name="maxTransId">The maximum trans identifier.</param>
        /// <returns>Data of system parlay monitor.</returns>
        private IEnumerable<RMS_Monitor_SystemParlay_Betlist> CreateTestData(
                                                                  string selection,
                                                                  string level,
                                                                  decimal stake,
                                                                  long maxTransId)
        {
            Random rnd = new Random();
            var count = rnd.Next(0, 5);
            var list = new List<RMS_Monitor_SystemParlay_Betlist>();

            for (int i = count; i > 0; i--)
            {
                int tempRefno = rnd.Next(1000000, 9999999);
                int temCombo = rnd.Next(3, 8);
                int tempLevel = rnd.Next(0, 10);
                decimal tempStake = Convert.ToDecimal(rnd.NextDouble() * 100);
                decimal tempOdds = Convert.ToDecimal(rnd.NextDouble() * 100);

                if (selection.Contains(temCombo.ToString()) &&
                    level.Contains(tempLevel.ToString()) &&
                    tempStake > stake)
                {
                    list.Add(new RMS_Monitor_SystemParlay_Betlist
                    {
                        Combo = temCombo,
                        Level = (byte)tempLevel,
                        Username = "test",
                        Stake = tempStake,
                        Odds = tempOdds,
                        NetWinAfterPT = tempStake * tempOdds,
                        RefNo = tempRefno,
                        TransDate = DateTime.Now,
                        TransId = maxTransId + i
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// Gets the name of the combo.
        /// </summary>
        /// <param name="combo">The combo.</param>
        /// <returns>The combo name.</returns>
        private string GetComboName(int combo)
        {
            string result = string.Empty;
            string format = "{0} ({1})";

            switch ((SystemParlayCombo)combo)
            {
                case SystemParlayCombo.Trixie:
                    result = string.Format(format, ViewLang.Trixie, combo);
                    break;

                case SystemParlayCombo.Yankee:
                    result = string.Format(format, ViewLang.Yankee, combo);
                    break;

                case SystemParlayCombo.Canadian:
                    result = string.Format(format, ViewLang.Canadian, combo);
                    break;

                case SystemParlayCombo.Heinz:
                    result = string.Format(format, ViewLang.Heinz, combo);
                    break;

                case SystemParlayCombo.SuperHeinz:
                    result = string.Format(format, ViewLang.SuperHeinz, combo);
                    break;

                case SystemParlayCombo.Goliath:
                    result = string.Format(format, ViewLang.Goliath, combo);
                    break;
            }

            return result;
        }
    }
}