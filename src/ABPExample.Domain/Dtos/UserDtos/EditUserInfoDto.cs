using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ABPExample.Domain.Models.Enum;

namespace ABPExample.Domain.Dtos.UserDtos
{
    public class EditUserInfoDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public EnumGender Gender { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Identity { get; set; }
    }
}
