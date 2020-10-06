using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.UserDtos
{
    public class AuthUserInfoDto
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

    }
}
