using ABPExample.Domain.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABPExample.Domain.Dtos.UserDtos
{
    public class UserInfoListSearchDto:PageSearchDto
    {
        public int? DepartmentId { get; set; }

        public bool? IsOther { get; set; }

    }
}
