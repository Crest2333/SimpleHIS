using ABPExample.Domain.Dtos.Appointment;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HIS.Domain.Dtos.Appointment;

namespace ABPExample.Application.Interface
{
    public interface IAppointmentApplication
    {
        Task<ModelResult> AddAppointment(AddAppointmentInfoDto inputDto);

        Task<ModelResult> BatchAddAppointment(List<AddAppointmentInfoDto> infoDtoList);

        Task<ModelResult> DeleteAppointment(int id);

        Task<ModelResult<AppointmentInfoDetailDto>> GetAppointmentInfoDetail(int id);

        Task<ModelResult<PageDto<AppointmentInfoListDto>>> GetAppointmentInfoList(AppointmentInfoListSearchDto param);

        Task<ModelResult<bool>> ChangeAppointmentStatus(EditAppointmentStatusInputDto inputDto);

        Task UpdateAppointmentStatusAsync();
        Task<ModelResult> EditAppointmentAsync(EditAppointmentInputDto inputDto);
    }
}
