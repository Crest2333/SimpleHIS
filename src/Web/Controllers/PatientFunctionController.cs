using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Appointment;
using ABPExample.Domain.Dtos.Common;
using ABPExample.Domain.Dtos.Doctor;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Public;
using ABPExample.Query.Common;
using HIS.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]

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
        [Authorize]
        public IActionResult DoctorList()
        {
            return View();
        }

        public async Task<JsonResult> GetDoctorList(GetDoctorInputDto param)
        {
            return Json(await _doctorApplication.GetDoctorListAsync(param));
        }

        [Authorize]

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

            var patientInfo = await _patientApplication.GetPatientByUserIdAsync(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value.ToInt());
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
               User.Claims.First(c => c.Type == "UserId").Value.ToInt()));

        }

        public async Task<JsonResult> GetPatientInfoDetail()
        {

            var patientInfo = await _patientApplication.GetPatientByUserIdAsync(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value.ToInt());
            return Json(patientInfo == null ? new ModelResult { IsSuccess = false, Message = "请先绑定患者信息" }
               : new ModelResult<PatientInfoDetailDto> { IsSuccess = true, Result = patientInfo });
        }
        [Authorize]
        [HttpPost]
        public async Task<JsonResult> GetAppointmentList(AppointmentInfoListSearchDto param)
        {
            var patientInfo = await _patientApplication.GetPatientByUserIdAsync(User.Claims.First(c => c.Type == "UserId").Value.ToInt());
            if (patientInfo == null)
                return Json(new ModelResult { IsSuccess = false, Message = "请先绑定患者信息" });

            param.PatientId = patientInfo.Id;
            return Json(await _appointmentApplication.GetAppointmentInfoList(param));
        }

        public async Task<JsonResult> EditPatientInfo(EditPatientInfoDto inputDto)
        {
            return Json(await _patientApplication.Edit(inputDto));
        }

        public async Task<IActionResult> Online()
        {
            var patientInfo =
                await _patientApplication.GetPatientByUserIdAsync(User.Claims.FirstOrDefault(c => c.Type == "UserId")
                    .Value.ToInt());
            if (patientInfo != null)
                ViewBag.Name = patientInfo.FullName;
            return View();
        }

        public async Task<JsonResult> GetChatLogByDoctorId(int doctorId)
        {
            return Json(await _patientUserApplication.GetChatLogByDoctorIdAsync(doctorId, User.Claims.First(c => c.Type == "UserId").Value.ToInt(), DateTime.Now.AddDays(-7)));
        }

        public async Task<JsonResult> GetAppointmentByPatientId(AppointmentInfoListSearchDto param)
        {
            var patientInfo = await _patientApplication.GetPatientByUserIdAsync(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value.ToInt());
            if (patientInfo == null)
                return Json(ModelResult<PageDto<AppointmentInfoListDto>>.Instance.Ok("",
                    new PageDto<AppointmentInfoListDto>(count: 0, list: new List<AppointmentInfoListDto>())));
            param.PatientId = patientInfo.Id;
            return Json(await _appointmentApplication.GetAppointmentInfoList(param));
        }
    }
}
