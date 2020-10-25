﻿using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ABPExample.Domain.Models
{
    public class RoleMapper:FullEntity
    {
        public string AccountNo { get; set; }

        public int RoleId { get; set; }
    }
}
