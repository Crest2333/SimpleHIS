using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Appointment;
using ABPExample.Domain.Dtos.MedicalAdvice;
using ABPExample.Query.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApplication _doctorApplication;
        private readonly IAppointmentApplication _appointmentApplication;

        public DoctorController(IDoctorApplication doctorApplication,IAppointmentApplication appointmentApplication)
        {
            _doctorApplication = doctorApplication;
            _appointmentApplication = appointmentApplication;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetSchedulingByUserId(int userId, DateTime? startDate, DateTime? endDate, int departmentId)
        {
            return Json(await _doctorApplication.GetSchedulingByUserId(userId, startDate, endDate,departmentId));
        }

        public IActionResult AppointmentList(int doctorId)
        {
            return View();
        }
        [Authorize]
        public IActionResult SchedulingList()
        {
            ViewBag.UseNo = User.Claims.First(c=>c.Type== "UserNo").Value;
            return View();
        }

        public async Task<JsonResult> GetAppointmentList(AppointmentInfoListSearchDto param)
        {
            param.DoctorId = User.Claims.First(c => c.Type == "UserId").Value.ToInt();

            return Json(await _appointmentApplication.GetAppointmentInfoList(param));
        }

        public IActionResult DoctorWork(int appointmentId)
        {
            ViewBag.AppointmentId = appointmentId;
            return View();
        }

        public async Task<JsonResult> AddOrEditMedicalAdvice(MedicalAdviceInputDto inputDto)
        {
            return Json(await _doctorApplication.AddOrEditMedicalAdviceAsync(inputDto));
        }

        [HttpGet]
        public async Task<JsonResult> GetMedicalAdvice(int appointmentId)
        {
            return Json(await _doctorApplication.GetMedicalAdviceAsync(appointmentId));
        }

    }
}
