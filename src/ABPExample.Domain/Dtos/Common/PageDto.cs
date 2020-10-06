using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.Common
{
    public class PageDto<T> where T:class
    {
        public int Count { get; set; }

        public IList<T> List { get; set; }
    }
}
