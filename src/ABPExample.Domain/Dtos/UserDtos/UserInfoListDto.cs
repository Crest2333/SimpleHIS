using ABPExample.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.UserDtos
{
    public class UserInfoListDto
    {
        public int Id { get; set; }

        public string UserAccount { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string UserIdentity { get; set; }

        public EnumGender Gender { get; set; }

        public string DepartmentName { get; set; }

        public string RoleName { get; set; }
        public string GenderDesc
        {
            get
            {
                switch (Gender)
                {
                    case EnumGender.Man:
                        return "男";
                    case EnumGender.Woman:
                        return "女";
                    case EnumGender.Other:
                        return "其他";
                    default:
                        return null;
                }
            }
        }
    }
}
