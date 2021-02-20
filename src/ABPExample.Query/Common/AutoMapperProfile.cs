using ABPExample.Domain.Dtos.Drug;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Dtos.Department;
using ABPExample.Domain.Dtos.MedicalHistory;
using ABPExample.Domain.Dtos.Role;
using AutoMapper;
using Volo.Abp.DependencyInjection;

namespace ABPExample.Query.Common
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddPatientInfoDto, Patients>();

            CreateMap<Patients,PatientInfoListDto >();

            CreateMap<Patients, PatientInfoDetailDto>();

            CreateMap<EditPatientInfoDto, Patients>();

            CreateMap<AddPastHistoryDto, PastHistories>();

            CreateMap<PastHistories, MedicalInfoDto>();

            CreateMap<EditMedicalInputDto, PastHistories>();

            CreateMap<AddDrugInputDto, Drug>();

            CreateMap<Drug, DrugInfoListDto>();

            CreateMap<EditDrugInputDto, Drug>();

            CreateMap<Drug, DrugInfoDetailDto>();

            CreateMap<Department, DepartmentInfoListDto>();

            CreateMap<Role, RoleInfoDto>();
        }
    }
}
