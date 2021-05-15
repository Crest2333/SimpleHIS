using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Appointment;
using ABPExample.Domain.Dtos.MedicalHistory;
using ABPExample.Domain.Dtos.Patient;
using ABPExample.Domain.Public;
using ABPExample.Query.Common;
using HIS.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class PAController : Controller
    {
        private readonly IAppointmentApplication _appointmentApplication;
        private readonly IPatientApplication _patientApplication;
        private readonly IMedicalHistoryApplication _medicalHistory;
        private readonly IUserApplication _userApplication;
        private readonly IPatientUserApplication _patientUserApplication;


        public PAController(IAppointmentApplication appointmentApplication, IPatientApplication patientApplication, IMedicalHistoryApplication medicalHistory
        ,IUserApplication userApplication,IPatientUserApplication patientUserApplication)
        {
            _appointmentApplication = appointmentApplication;
            _patientApplication = patientApplication;
            _medicalHistory = medicalHistory;
            _userApplication = userApplication;
            _patientUserApplication = patientUserApplication;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PatientInfoList()
        {
            return View();
        }

        public IActionResult PatientDetail(int patientId)
        {
            ViewBag.Id = patientId;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetAppointmentList(AppointmentInfoListSearchDto param)
        {
            return Json(await _appointmentApplication.GetAppointmentInfoList(param));
        }

        [HttpGet]
        public async Task<JsonResult> GetAppointmentDetailInfo(int id)
        {
            return Json(await _appointmentApplication.GetAppointmentInfoDetail(id));
        }

        [HttpGet]
        public async Task<JsonResult> DeletePatient(int id)
        {
            return Json(await _patientApplication.Delete(id));
        }

        [HttpGet]
        public async Task<JsonResult> GetPatientInfoDetail(int id)
        {
            return Json(await _patientApplication.Detail(id));
        }

        [HttpPost]
        public async Task<JsonResult> GetPatientInfoList(PatientSearchDto param)
        {
            return Json(await _patientApplication.List(param));
        }

        [HttpPost]
        public async Task<JsonResult> AddPatient(AddPatientInfoDto inputDto)
        {
            return Json(await _patientApplication.Add(inputDto));
        }

        public async Task<JsonResult> EditPatientInfo(EditPatientInfoDto inputDto)
        {
            return Json(await _patientApplication.Edit(inputDto));
        }

        public async Task<JsonResult> DeleteHistory(int patientId)
        {
            return Json(await _medicalHistory.Delete(patientId));
        }

        [HttpPost]
        public async Task<JsonResult> AddMedicalHistory(AddPastHistoryDto inputDto)
        {
            inputDto.CreateBy =
                (await _userApplication.GetUserInfoDetail(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value
                    .ToInt())).Result.UserAccount;
            return Json(await _patientApplication.AddIllnessHistory(inputDto));
        }

        [HttpPost]

        public async Task<JsonResult> EditMedicalHistory(EditMedicalInputDto input)
        {
            return Json(await _medicalHistory.Edit(input));
        }

        public async Task<JsonResult> GetMedicalHistoryByPatientId(GetMedicalInputDto param)
        {
            var patientInfo = await _patientApplication.GetPatientByUserIdAsync(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value.ToInt());
           
            return Json(await _medicalHistory.List(param));
        }

        public async Task<JsonResult> GetMedicalHistoryInfoById(int id)
        {
            return Json(await _medicalHistory.Detail(id));
        }

        #region  预约

        public IActionResult AppointmentInfoList()
        {
            return View();
        }

        public IActionResult AddAppointment(int patientId)
        {
            return View();
        }

        public IActionResult AppointmentDetail(int appointmentId)
        {
            ViewBag.AppointmentId = appointmentId;
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> AddAppointment(AddAppointmentInfoDto inputDto)
        {
            return Json(await _appointmentApplication.AddAppointment(inputDto));
        }

        [HttpPost]
        public async Task<JsonResult> DeleteAppointment(int appointmentId)
        {
            return Json(await _appointmentApplication.DeleteAppointment(appointmentId));
        }

        [HttpPost]
        public async Task<JsonResult> ChangeAppointmentStatus(EditAppointmentInputDto inputDto)
        {
            return Json(await _appointmentApplication.ChangeAppointmentStatus(inputDto));
        }

       
        #endregion

    }
}
