using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Public
{
    public class ModelResult
    {
        public static ModelResult Instance
        {
            get { return new ModelResult(); }
        }
        public ModelResult()
        {
            Code = 200;
            IsSuccess = true;
        }

        public ModelResult Error(string msg )
        {
            this.Code = 500;
            this.Message = msg;
            return this;
        }
        public ModelResult Ok(string msg )
        {
            Code = 200;
            IsSuccess = true;
            Message = msg;
            return this;
        }

        public bool IsSuccess { get; set; }

        public int Code { get; set; }

        public string Message { get; set; }

    }

    public class ModelResult<T>:ModelResult
    {
        public static  ModelResult<T> Instance
        {
            get { return new ModelResult<T>(); }
        }
        public ModelResult<T> Ok(string msg,T data)
        {
            Result = data;
            Code = 200;
            IsSuccess = true;
            Message = msg;
            return this;
        }

        public T Result { get; set; }
    }
}
