namespace ParlayRMS.Models.Functions
{
    using ParlayRMS.Models;

    public class FunctionPolicy
    {
        public int FunctionId { get; set; }

        public string FunctionName { get; set; }

        public string FunctionKey { get; set; }

        public Policy Policy { get; set; }
    }
}