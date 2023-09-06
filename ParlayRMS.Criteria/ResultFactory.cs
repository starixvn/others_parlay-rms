namespace ParlayRMS.Criteria
{
    public static class ResultFactory
    {
        public static Result From(IResult obj)
        {
            return new Result
            {
                Code = obj.ErrCode,
                Message = obj.ErrMsg
            };
        }
    }
}
