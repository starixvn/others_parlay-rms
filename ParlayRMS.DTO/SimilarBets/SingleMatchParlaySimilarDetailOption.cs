namespace ParlayRMS.DTO.SimilarBets
{
    public class SingleMatchParlaySimilarDetailOption
    {
        public int CustId { get; set; }

        public int OriginalCustId { get; set; }

        public long TransId { get; set; }

        public long OriginalTransId { get; set; }

        public string Username { get; set; }

        public string Selection { get; set; }
    }
}
