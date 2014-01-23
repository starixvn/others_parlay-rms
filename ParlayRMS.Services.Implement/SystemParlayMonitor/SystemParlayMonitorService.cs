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
                item.Odds = Math.Round(item.Odds, 2);
                item.Stake = Math.Round(item.Stake, 2);
                item.MaxPayout = Math.Round(item.Odds * item.Stake, 2);
                item.ComboName = GetComboName(item.Combo);
            }

            return data;
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
                        MaxPayout = tempStake * tempOdds,
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

            switch ((SystemParlayCombo)combo)
            {
                case SystemParlayCombo.Trixie:
                    result = ViewLang.Trixie;
                    break;

                case SystemParlayCombo.Yankee:
                    result = ViewLang.Yankee;
                    break;

                case SystemParlayCombo.Canadian:
                    result = ViewLang.Canadian;
                    break;

                case SystemParlayCombo.Heinz:
                    result = ViewLang.Heinz;
                    break;

                case SystemParlayCombo.SuperHeinz:
                    result = ViewLang.SuperHeinz;
                    break;

                case SystemParlayCombo.Goliath:
                    result = ViewLang.Goliath;
                    break;
            }

            return result;
        }
    }
}