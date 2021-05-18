using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.UserDtos
{
    public class EditPassWordDto
    {
        public int Id { get; set; }

        public string ConfirmPassWord { get; set; }

        public string NewPassWord { get; set; }
    }
}
