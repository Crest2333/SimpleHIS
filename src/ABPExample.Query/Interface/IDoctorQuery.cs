using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Doctor;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Scheduling;
using HIS.Domain.Dtos.Doctor;

namespace ABPExample.Query.Interface
{
    public interface IDoctorQuery
    {
        Task<ModelResult<PageDto<DoctorInfoDto>>> ListAsync(GetDoctorInputDto param);

        Task<ModelResult<DoctorInfoDto>> DetailAsync(int id);

        Task<ModelResult<List<SchedulingDto>>> GetSchedulingByUserId(int userId, DateTime? startDate, DateTime? endDate, int departmentId);

        /// <summary>
        ///    获取医生信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ModelResult<PageDto<DoctorEntityDto>>> GetDoctorListAsync(GetDoctorInputDto param);
    }
}
