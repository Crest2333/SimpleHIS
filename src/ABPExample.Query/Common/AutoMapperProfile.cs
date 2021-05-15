using ABPExample.Domain.Dtos.Drug;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using ABPExample.Domain.Dtos.Department;
using ABPExample.Domain.Dtos.MedicalAdvice;
using ABPExample.Domain.Dtos.MedicalHistory;
using ABPExample.Domain.Dtos.Role;
using ABPExample.Domain.Dtos.Scheduling;
using AutoMapper;
using HIS.Domain.Dtos.Chat;
using HIS.Domain.Dtos.Doctor;
using HIS.Domain.Dtos.PatientUser;
using HIS.Domain.Models;
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

            CreateMap<Role, RoleInfoDto>()
                .ForMember(dto=>dto.RoleId,src=>src.MapFrom(option=>option.Id));

            CreateMap<AddSchedulingInputDto, Scheduling>();

            CreateMap<MedicalAdviceInputDto, MedicalAdvice>();

            CreateMap<RegisterInputDto, PatientUser>()
                .ForMember(dto=>dto.UserPwd,src=>src.MapFrom(option=>option.PassWord));

            CreateMap<DoctorInfoInputDto, Doctor>()
                .ForMember(dto => dto.DoctorImg, src => src.MapFrom(option => option.ImgUrl));;

            CreateMap<ChatLogDto, ChatLog>();
            CreateMap<ChatLog, ChatLogDto>();

        }
    }
}
