using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Appointment;
using ABPExample.Domain.Dtos.Doctor;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Public;
using ABPExample.Query.Common;
using HIS.Application.Interface;
using HIS.Query.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace PatientWeb.Controllers
{
   public class PatientFunctionController : AbpController
   {
      private readonly IDoctorApplication _doctorApplication;
      private readonly IDepartmentApplication _departmentApplication;
      private readonly IAppointmentApplication _appointmentApplication;
      private readonly IPatientApplication _patientApplication;
      private readonly IPatientUserApplication _patientUserApplication;

      public PatientFunctionController(IDoctorApplication doctorApplication,
         IDepartmentApplication departmentApplication,
         IAppointmentApplication appointmentApplication,
         IPatientApplication patientApplication,
         IPatientUserApplication patientUserApplication)
      {
         _doctorApplication = doctorApplication;
         _departmentApplication = departmentApplication;
         _appointmentApplication = appointmentApplication;
         _patientApplication = patientApplication;
         _patientUserApplication = patientUserApplication;
      }
      public IActionResult DoctorList()
      {
         return View();
      }

      public async Task<JsonResult> GetDoctorList(GetDoctorInputDto param)
      {
         return Json(await _doctorApplication.GetDoctorListAsync(param));
      }

      public IActionResult AddAppointment(int doctorId)
      {
         return View();
      }

      public async Task<JsonResult> GetDepartmentByDoctorId(int doctorId)
      {
         return Json(await _departmentApplication.GetDepartmentByDoctorIdAsync(doctorId));
      }

      public async Task<JsonResult> GetSchedulingByUserId(int userId, DateTime? startDate, DateTime? endDate, int departmentId)
      {
         return Json(await _doctorApplication.GetSchedulingByUserId(userId, startDate, endDate, departmentId));
      }
      [Authorize]
      [HttpPost]
      public async Task<JsonResult> AddAppointment(AddAppointmentInfoDto inputDto)
      {

         var patientInfo = await _patientApplication.GetPatientByUserIdAsync(User.Claims.FirstOrDefault(c => c.Type == "Id").Value.ToInt());
         if (patientInfo == null)
            return Json(new ModelResult { IsSuccess = false, Message = "请先绑定患者信息" });
         inputDto.PatientId = patientInfo.Id;

         return Json(await _appointmentApplication.AddAppointment(inputDto));
      }
      [Authorize]
      [HttpPost]
      public async Task<JsonResult> AddPatient(AddPatientInfoDto inputDto)
      {

         return Json(await _patientUserApplication.AddPatientMapperAsync(inputDto,
            User.Claims.First(c => c.Type == "Id").Value.ToInt()));

      }

      public async Task<JsonResult> GetPatientInfoDetail()
      {

         var patientInfo = await _patientApplication.GetPatientByUserIdAsync(User.Claims.FirstOrDefault(c => c.Type == "Id").Value.ToInt());
         return Json(patientInfo == null ? new ModelResult { IsSuccess = false, Message = "请先绑定患者信息" }
            : new ModelResult<PatientInfoDetailDto> { IsSuccess = true, Result = patientInfo });
      }
      [Authorize]
      [HttpPost]
      public async Task<JsonResult> GetAppointmentList(AppointmentInfoListSearchDto param)
      {
         var patientInfo = await _patientApplication.GetPatientByUserIdAsync(User.Claims.First(c => c.Type == "Id").Value.ToInt());
         if (patientInfo == null)
            return Json(new ModelResult { IsSuccess = false, Message = "请先绑定患者信息" });

         param.PatientId = patientInfo.Id;
         return Json(await _appointmentApplication.GetAppointmentInfoList(param));
      }

      public async Task<JsonResult> EditPatientInfo(EditPatientInfoDto inputDto)
      {
         return Json(await _patientApplication.Edit(inputDto));
      }

      public IActionResult Online()
      {
          return View();
      }
   }
}
