using ABPExample.Domain.Dtos.Appointment;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ABPExample.Query.Interface
{
    public interface IPAQuery
    {
        Task<ModelResult> AddAppointment(AddAppointmentInfoDto inputDto);

        Task<ModelResult> BatchAddAppointment(List<AddAppointmentInfoDto> infoDtoList);

        Task<ModelResult> DeleteAppointment(int id);

        Task<ModelResult<AppointmentInfoDetailDto>> GetAppointmentInfoDetail(int id);

        Task<ModelResult<PageDto<AppointmentInfoListDto>>> GetAppointmentInfoList(AppointmentInfoListSearchDto param);

        Task<ModelResult<bool>> ChangeAppointmentStatus(EditAppointmentInputDto inputDto);
    }
}
