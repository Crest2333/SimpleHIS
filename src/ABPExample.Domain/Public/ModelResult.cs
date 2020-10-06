using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Public
{
    public class ModelResult
    {
        public ModelResult()
        {
            Code = 200;
            IsSuccess = true;
        }

        public bool IsSuccess { get; set; }

        public int Code { get; set; }

        public string Message { get; set; }

    }

    public class ModelResult<T>:ModelResult
    {
        public T Result { get; set; }
    }
}
