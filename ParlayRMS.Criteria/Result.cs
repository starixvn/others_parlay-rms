namespace ParlayRMS.Criteria
{
    public interface IResult
    {
        int ErrCode { get; set; }

        string ErrMsg { get; set; }

    }

    public class Result
    {
        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public Result()
            : this(0, string.Empty)
        {
        }

        public static Result Empty => new Result();

        public int Code { get; set; }

        public string Message { get; set; }

        public bool IsSuccessful => Code == 0;
    }
}
