namespace ParlayRMS.DTO.Users
{
    public class UserLoginParameter
    {
        public string Username { get; set; }

        public string Userpwd { get; set; }

        public string LoginIp { get; set; }

        public string LoginDomain { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public string Language { get; set; }
    }
}