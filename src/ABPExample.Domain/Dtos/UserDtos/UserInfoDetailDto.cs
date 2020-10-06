using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.UserDtos
{
    public class UserInfoDetailDto
    {
        public int Id { get; set; }

        public string UserAccount { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string UserPwd { get; set; }

        public string Email { get; set; }

        public string UserImg { get; set; }

        public string UserIdentity { get; set; }

        public int Gender { get; set; }
    }
}
