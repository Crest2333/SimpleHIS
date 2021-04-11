using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HIS.Domain.Dtos.PatientUser;

namespace HIS.Query.Interface
{
    public interface IPatientUserQuery
    {
        Task<Dictionary<int, string>> AuthenticationAsync(string account, string passWord);

        Task<bool> RegisterAsync(RegisterInputDto input);

        Task<bool> EditPatientUserAsync(PatientInputDto input);

        Task<bool> AddPatientMapperAsync(int userId, int patientId);
    }
}
