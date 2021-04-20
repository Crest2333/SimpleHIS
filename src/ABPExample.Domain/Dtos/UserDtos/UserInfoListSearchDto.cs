using ABPExample.Domain.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.UserDtos
{
    public class UserInfoListSearchDto:PageSearchDto
    {
        public int? DepartmentId { get; set; }

        public string IdentityId { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public int? Gender { get; set; }

        public bool? IsOther { get; set; }

        public bool? IsRoleUser { get; set; }

        public int? RoleId { get; set; }

    }
}
