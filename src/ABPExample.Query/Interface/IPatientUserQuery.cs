using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HIS.Domain.Dtos.PatientUser;
using HIS.Domain.Models;

namespace HIS.Query.Interface
{
    public interface IPatientUserQuery
    {
        Task<Dictionary<int, string>> AuthenticationAsync(string account, string passWord);

        Task<bool> RegisterAsync(RegisterInputDto input);

        Task<bool> EditPatientUserAsync(PatientInputDto input);

        Task<bool> AddPatientMapperAsync(int userId, int patientId);
        Task<PatientUser> GetPatientUserByEmail(string inputDtoEmail);

        Task<int> UpdateAsync(PatientUser entity);
    }
}
