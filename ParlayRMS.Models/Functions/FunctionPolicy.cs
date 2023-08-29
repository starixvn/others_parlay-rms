namespace ParlayRMS.Models.Functions
{
    public class FunctionPolicy
    {
        public int FunctionId { get; set; }

        public string FunctionName { get; set; }

        public string FunctionKey { get; set; }

        public Policy Policy { get; set; }
    }
}
