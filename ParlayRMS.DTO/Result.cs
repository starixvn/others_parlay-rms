// --------------------------------------------------------------------------
// <copyright file="Result.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
// </history>

namespace ParlayRMS.DTO
{
    /// <summary>
    /// Result model.
    /// </summary>
    public class Result
    {
        public Result(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public Result()
            : this(-1, string.Empty)
        {
        }

        public static Result Empty
        {
            get
            {
                return new Result();
            }
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public bool IsSuccessful
        {
            get
            {
                return this.Code == 0;
            }
        }
    }
}