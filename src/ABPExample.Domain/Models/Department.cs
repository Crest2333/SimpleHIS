using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Models
{
    public class Department:FullEntity
    {
        public string Name { get; set; }

        public string Img { get; set; }

        public string Describe { get; set; }
    }
}
