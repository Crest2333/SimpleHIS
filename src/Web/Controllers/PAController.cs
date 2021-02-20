using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Appointment;
using ABPExample.Domain.Dtos.MedicalHistory;
using ABPExample.Domain.Dtos.Patient;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class PAController : Controller
    {
        private readonly IAppointmentApplication _appointmentApplication;
        private readonly IPatientApplication _patientApplication;
        private readonly IMedicalHistoryApplication _medicalHistory;


        public PAController(IAppointmentApplication appointmentApplication, IPatientApplication patientApplication, IMedicalHistoryApplication medicalHistory)
        {
            _appointmentApplication = appointmentApplication;
            _patientApplication = patientApplication;
            _medicalHistory = medicalHistory;
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

        [HttpPost]
        public async Task<JsonResult> AddMedicalHistory(AddPastHistoryDto inputDto)
        {
            return Json(await _patientApplication.AddIllnessHistory(inputDto));
        }

        [HttpPost]

        public async Task<JsonResult> EditMedicalHistory(EditMedicalInputDto input)
        {
            return Json(await _medicalHistory.Edit(input));
        }

        public async Task<JsonResult> GetMedicalHistoryByPatientId(GetMedicalInputDto param)
        {
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

        [HttpPost]
        public async Task<JsonResult> AddAppointment(AddAppointmentInfoDto inputDto)
        {
            return Json(await _appointmentApplication.AddAppointment(inputDto));
        }
        #endregion
    }
}
