using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Dtos.UserDtos;
using ABPExample.Domain.Public;
using RegisterInputDto = HIS.Domain.Dtos.PatientUser.RegisterInputDto;

namespace HIS.Application.Interface
{
   public interface IPatientUserApplication
   {
      Task<ModelResult> RegisterAsync(RegisterInputDto input);

      Task<ModelResult> LoginAsync(LoginInputDto input);

      /// <summary>
      ///   添加对应关系
      /// </summary>
      /// <param name="inputDto"></param>
      /// <param name="toInt"></param>
      /// <returns></returns>
      Task<ModelResult> AddPatientMapperAsync(AddPatientInfoDto inputDto, int toInt);
   }
}
