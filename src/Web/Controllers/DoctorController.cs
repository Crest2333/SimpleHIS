﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Appointment;
using ABPExample.Domain.Dtos.Doctor;
using ABPExample.Domain.Dtos.MedicalAdvice;
using ABPExample.Query.Common;
using HIS.Domain.Dtos.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class DoctorController : Controller
    {
        private readonly IDoctorApplication _doctorApplication;
        private readonly IAppointmentApplication _appointmentApplication;
        private readonly IUserApplication _userApplication;

        public DoctorController(IDoctorApplication doctorApplication, IAppointmentApplication appointmentApplication,IUserApplication userApplication)
        {
            _doctorApplication = doctorApplication;
            _appointmentApplication = appointmentApplication;
            _userApplication = userApplication;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetSchedulingByUserId(int userId, DateTime? startDate, DateTime? endDate, int departmentId)
        {
            return Json(await _doctorApplication.GetSchedulingByUserId(userId, startDate, endDate, departmentId));
        }

        public IActionResult AppointmentList(int doctorId)
        {
            return View();
        }
        [Authorize]
        public IActionResult SchedulingList()
        {
            ViewBag.UseNo = User.Claims.First(c => c.Type == "UserNo").Value;
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

        [HttpPost]
        public async Task<JsonResult> EditDoctorInfo(DoctorInfoInputDto inputDto, IFormFile file)
        {
            inputDto.UserId = Convert.ToInt32(Request.Form["UserId"]);
            inputDto.Introduce = Request.Form["Introduce"].ToString();
            return Json(await _doctorApplication.EditDoctorInfoAsync(inputDto, file));
        }

        [HttpPost]
        public async Task<JsonResult> GetDoctorInfoList(GetDoctorInputDto param)
        {
            param.IsManager = true;
            return Json(await _doctorApplication.GetDoctorListAsync(param));
        }

        public async Task<JsonResult> DoctorInfoDetail(int userId)
        {
            return Json(await _doctorApplication.GetDoctorInfoDetailAsync(userId));
        }

        public async Task<IActionResult> OnLine()
        {
           var userInfo= 
                await _userApplication.GetUserInfoDetail(User.Claims.First(c => c.Type == "UserId").Value.ToInt());
           if (userInfo.Result != null)
               ViewBag.Name = userInfo.Result.UserName;
            return View();
        }

        [Authorize]
        public async Task<JsonResult> GetOnlineUser()
        {
            
            return Json(await _doctorApplication.GetOnlineUserAsync(User.Claims.First(c=>c.Type=="UserId").Value.ToInt()));
        }

        public async Task<JsonResult> GetChatLogByUserId(int userId)
        {
            return Json(await _doctorApplication.GetChatLogByUserIdAsync(userId,
                User.Claims.FirstOrDefault(c => c.Type == "UserId").Value.ToInt()));
        }
    }
}
