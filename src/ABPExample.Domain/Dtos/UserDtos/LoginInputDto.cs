using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.UserDtos
{
    public class LoginInputDto
    {
        public string AccountNo { get; set; }

        public string PassWord { get; set; }
    }
}
