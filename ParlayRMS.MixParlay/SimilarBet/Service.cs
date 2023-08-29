using Fanex.Data;
using Fanex.Data.Repository;
using System.Linq;

namespace ParlayRMS.MixParlay.SimilarBet
{
    public class Service : IService
    {
        public Ticket GetTicket(long transId)
        {
            var criteria = new Criteria
            {
                TransId = transId
            };

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var ticket = multi.Read<Ticket>().First();
                    var ticketDetails = multi.Read<TicketDetail>();
                    ticket.Details = ticketDetails;
                    return ticket;
                }
            }
        }
    }
}
