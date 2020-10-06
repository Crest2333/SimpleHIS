using ABPExample.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ABPExample.Domain.Dtos.UserDtos
{
    public class AddUserInputDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public EnumGender Gender { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public  string  Identity { get; set; }
    }
}
