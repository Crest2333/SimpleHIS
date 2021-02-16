
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Models
{
    public class Users:FullEntity
    {
        public string UserAccount { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string UserPwd { get; set; }

        public string Email { get; set; }

        public string UserImg { get; set; }

        public string UserIdentity { get; set; }

        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Man,
        Woman,
        Other
    }
}
