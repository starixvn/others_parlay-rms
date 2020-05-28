using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using ParlayRMS.DataAccess.Implement.BetDetails;
using ParlayRMS.DTO.BetDetails;
using ParlayRMS.Services.Implement.BetList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParlayRMS.Services.Implement.BetDetails
{
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
            foreach (var ticket in tickets)
            {
                if (tickets.Last() == ticket)
                {
                    builder.AddChoice(
                        ticket,
                        ticketHelper,
                        null,
                        string.Format("<div id='{0}'><div class='ticketDetail'>", ticket.BetId),
                        string.Format("</div><div class='ticketStatus'><div class='divstatus'>[{0}]</div></div></div>", ticket.Status),
                        false);
                }
                else
                {
                    builder.AddChoice(
                        ticket,
                        ticketHelper,
                        null,
                        string.Format("<div id='{0}'><div class='ticketDetail'>", ticket.BetId),
                        string.Format("</div><div class='ticketStatus'><div class='divstatus'>[{0}]</div></div></div><div class='line'></div>", ticket.Status),
                        false);
                }
            }

            var comboTypeAndStake = GetComboTypeAndStake(tickets, betInfo);

            return new ParlayBetDetails
            {
                TicketDetail = builder.GetBetList(),
                ComboTypes = comboTypeAndStake
            };
        }

        public string GetChoice(long transId)
        {
            return GetChoiceForSingleMatchParlayDetailPopup(transId);
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

        private static List<string> GetComboTypeAndStake(IEnumerable<ITicket> tickets, BetInfo betInfo)
        {
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

        private string GetChoiceForSingleMatchParlayDetailPopup(long transId)
        {
            var model = _betlistService.RMS_BetList_BetBuilder_MatchDetail(transId).FirstOrDefault();

            if (model == null)
            {
                return string.Empty;
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

            return document.OuterHtml;
        }
    }
}
