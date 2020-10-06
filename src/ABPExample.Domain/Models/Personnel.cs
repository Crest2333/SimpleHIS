using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Models
{
    public class Personnel:FullEntity
    {
        public string Name { get; set; }

        public int MyProperty { get; set; }
    }
}
