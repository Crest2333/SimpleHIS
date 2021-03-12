using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Appointment;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Public;
using ABPExample.Query.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Application.Application
{
    public class AppointmentApplication: IAppointmentApplication,ITransientDependency
    {
        private readonly IPAQuery _pAQuery;

        public AppointmentApplication( IPAQuery pAQuery)
        {
            _pAQuery = pAQuery;
        }

        public async Task<ModelResult> AddAppointment(AddAppointmentInfoDto inputDto)
        {
            return await _pAQuery.AddAppointment(inputDto);
        }

        public async Task<ModelResult> BatchAddAppointment(List<AddAppointmentInfoDto> infoDtoList)
        {
            return await _pAQuery.BatchAddAppointment(infoDtoList);
        }

        public async Task<ModelResult> DeleteAppointment(int id)
        {
            return await _pAQuery.DeleteAppointment(id);
        }

        public async Task<ModelResult<AppointmentInfoDetailDto>> GetAppointmentInfoDetail(int id)
        {
            return await _pAQuery.GetAppointmentInfoDetail(id);
        }

        public async Task<ModelResult<PageDto<AppointmentInfoListDto>>> GetAppointmentInfoList(AppointmentInfoListSearchDto param)
        {
            return await _pAQuery.GetAppointmentInfoList(param);
        }

        public async Task<ModelResult<bool>> ChangeAppointmentStatus(EditAppointmentInputDto inputDto)
        {

            return await _pAQuery.ChangeAppointmentStatus(inputDto);
        }
    }
}
