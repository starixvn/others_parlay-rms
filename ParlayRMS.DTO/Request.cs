namespace ParlayRMS.DTO
{
    public class Request
    {
        public string Ip { get; set; }

        public string Domain { get; set; }

        public string Browser { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public string Language => "en-US";
    }
}
