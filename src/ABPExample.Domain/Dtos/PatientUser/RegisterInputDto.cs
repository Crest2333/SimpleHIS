using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HIS.Domain.Dtos.PatientUser
{
    public class RegisterInputDto
    {
        [Required(ErrorMessage = "电话号码必填")]
        [DisplayName("电话号码")]

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "邮箱必填")]
        [DisplayName("邮箱")]
        public string Email { get; set; }

        [Required(ErrorMessage = "密码必填")]
        [DisplayName("密码")]
        public string PassWord { get; set; }

    }
}
