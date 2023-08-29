namespace ParlayRMS.Services.BetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetList;
    using DataAccess.BetDetails;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fizzler.Systems.HtmlAgilityPack;
    using HtmlAgilityPack;
    using ParlayRMS.DTO.BetDetails;
    using ParlayRMS.DTO.BetList;

    public class BetDetailsService : BaseService
    {
        private readonly BetDetailsDataAccess _dataAccess = new BetDetailsDataAccess();

        private readonly BetListService _betlistService = new BetListService();

        public ParlayBetDetails GetBetDetail(long transId, long refNo)
        {
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = true };

            var data = _dataAccess.RMS_Rpt_Parlay_Bet_Details(transId, refNo);
            var tickets = _betlistService.Map(refNo, data);
            var betInfo = _dataAccess.GetBetInfo(transId);

            for (int i = 0; i < tickets.Item2.Count; i++)
            {
                var blackListLeague = tickets.Item1[i] ? "blackListClass blackListMargin" : string.Empty;
                var ticket = tickets.Item2[i];
                if (tickets.Item2.Last() == ticket)
                {
                    builder.AddChoice(
                        ticket,
                        ticketHelper,
                        null,
                        string.Format("<div  class='" + blackListLeague + "'></div><div id='{0}'><div class='ticketDetail'>", ticket.BetId),
                        string.Format("</div><div class='ticketStatus'><div class='divstatus'>[{0}]</div></div></div>", ticket.Status),
                        false);
                }
                else
                {
                    builder.AddChoice(
                        ticket,
                        ticketHelper,
                        null,
                        string.Format("<div  class='" + blackListLeague + "'></div><div id='{0}'><div class='ticketDetail'>", ticket.BetId),
                        string.Format("</div><div class='ticketStatus'><div class='divstatus'>[{0}]</div></div></div><div class='line'></div>", ticket.Status),
                        false);
                }
            }

            var comboTypeAndStake = GetComboTypeAndStake(tickets.Item2, betInfo);

            return new ParlayBetDetails
            {
                TicketDetail = builder.GetBetList(),
                ComboTypes = comboTypeAndStake
            };
        }

        public Tuple<bool, string> GetChoiceForSingleMatchParlay(long transId)
        {
            return GetChoiceForSingleMatchParlayDetailPopup(transId);
        }

        public Tuple<bool, string> GetPenaltyShootoutDetail(long transId)
        {
            return GetPenaltyShootoutParlayDetailPopup(transId);
        }

        public Tuple<List<string>, List<string>> GetSelectionAndResult(int custId, long transId, string username)
        {
            var criteria = new RMS_BetList_BetBuilder_DetailCriteria
            {
                CustId = custId,
                TransId = transId,
                Username = username
            };

            var selections = _betlistService.RMS_BetList_BetBuilder_Detail(criteria);
            var selectionNames = new List<string>();
            var resultTexts = new List<string>();
            if (selections.Any())
            {
                foreach (var s in selections)
                {
                    var name = Utility.CacheRef.RemotingRefService.GetBetBuilderSelectionNames(new[] { s.SelectionType }, "en-US");
                    selectionNames.Add(name.FirstOrDefault());
                    resultTexts.Add(s.Result);
                }
            }

            return new Tuple<List<string>, List<string>>(selectionNames, resultTexts);
        }

        public List<RMS_BetList_BetBuilder_DetailModel> GetSingleMatchParlaySelection(int custId, long transId)
        {
            var criteria = new RMS_BetList_BetBuilder_DetailCriteria
            {
                CustId = custId,
                TransId = transId
            };
            return _betlistService.RMS_BetList_BetBuilder_Detail(criteria);
        }

        public Tuple<List<bool>, List<ITicket>> CreateChoice(long transId, long refNo)
        {
            return _betlistService.CreateChoice(transId, refNo);
        }

        public RMS_Monitor_Parlay_BetInfo GetBetInfo(long transId)
        {
            return _betlistService.GetBetInfo(transId);
        }

        private static List<string> GetComboTypeAndStake(IEnumerable<ITicket> tickets, BetInfo betInfo)
        {
            if (betInfo == null)
            {
                return new List<string>();
            }
            var comboTypes = Utility.CacheRef.GetCombinationTypeCombo(betInfo.BetTeam, betInfo.BetCheck, betInfo.Hdp1, betInfo.Hdp2);
            var comboTypesAndStake = new List<string>();
            var hasStaketickets = tickets.Where(m => m.Stake > 0).OrderBy(m => m.MatchId).ToList();
            var count = Math.Min(comboTypes?.Length ?? 0, hasStaketickets.Count());
            for (var i = 0; i < count; i++)
            {
                comboTypesAndStake.Add(comboTypes[i] + " - " + FormatStake(hasStaketickets[i].Stake));
            }

            return comboTypesAndStake;
        }

        private static string FormatStake(decimal stake)
        {
            return stake < 10
                ? $"{stake:#,0.00}"
                : $"{Math.Floor(stake):#,0}";
        }

        private Tuple<bool, string> GetChoiceForSingleMatchParlayDetailPopup(long transId)
        {
            var model = _betlistService.RMS_BetList_BetBuilder_MatchDetail(transId).FirstOrDefault();

            if (model == null)
            {
                return new Tuple<bool, string>(false, string.Empty);
            }

            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = false };
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            var ticket = new Ticket
            {
                HomeId = model.HomeId,
                AwayId = model.AwayId,
                SportTypeId = model.SportType,
                LeagueId = model.LeagueId,
                EventDate = model.EventDate,
                TransId = model.TransId,
                BetTypeId = (int)Models.BetType.SingleMatchParlay
            };

            var choice = builder.BuildChoice(ticket, ticketHelper, null, false);
            var html = new HtmlDocument();
            html.LoadHtml(choice);
            var document = html.DocumentNode;
            var matchNode = document.QuerySelector(".match");
            var leagueNode = document.QuerySelector(".league");
            var eventDateNode = document.QuerySelector(".event-date");
            if (eventDateNode != null)
            {
                eventDateNode.Remove();
            }

            document.RemoveAllChildren();

            if (matchNode != null)
            {
                document.ChildNodes.Add(matchNode);
            }

            if (leagueNode != null)
            {
                document.ChildNodes.Add(leagueNode);
            }

            return new Tuple<bool, string>(model.IsBlacklistLeague, document.OuterHtml);
        }

        private Tuple<bool, string> GetPenaltyShootoutParlayDetailPopup(long transId)
        {
            var model = _betlistService.RMS_BetList_PenaltyShootout_MatchDetail(transId).FirstOrDefault();

            if (model == null)
            {
                return new Tuple<bool, string>(false, string.Empty);
            }

            ITicketHelper ticketHelper = new TicketHelper { IsShowOddsWithChoice = false };
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            var ticket = new Ticket
            {
                HomeId = model.HomeId,
                AwayId = model.AwayId,
                SportTypeId = model.SportType,
                LeagueId = model.LeagueId,
                EventDate = model.EventDate,
                TransId = model.TransId,
                BetTypeId = 376,
                BetCheck = model.BetCheck,
                BetTeam = model.BetTeam
            };

            var choice = builder.BuildChoice(ticket, ticketHelper, null, false);
            var html = new HtmlDocument();
            html.LoadHtml(choice);
            var document = html.DocumentNode;
            var bettype = document.QuerySelector(".bettype");
            var eventDateNode = document.QuerySelector(".event-date");
            if (eventDateNode != null)
            {
                eventDateNode.Remove();
            }

            if (bettype != null)
            {
                bettype.Remove();
            }

            return new Tuple<bool, string>(model.IsBlacklistLeague, document.OuterHtml);
        }
    }
}
