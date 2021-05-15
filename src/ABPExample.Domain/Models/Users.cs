
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ABPExample.Domain.Models
{
    public class Users : FullEntity
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
        [Description("男")]
        Man = 1,
        [Description("女")]
        Woman = 2,
        [Description("其他")]
        Other = 3,
    }
}
