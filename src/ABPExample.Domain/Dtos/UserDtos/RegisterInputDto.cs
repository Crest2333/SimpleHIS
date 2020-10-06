using ABPExample.Domain.Models.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.UserDtos
{
    public class RegisterInputDto
    {
        public string Name { get; set; }

        public EnumGender Gender { get; set; }

        public string Identity { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public IFormFile Img { get; set; }

        public string Introduction { get; set; }
    }
}
