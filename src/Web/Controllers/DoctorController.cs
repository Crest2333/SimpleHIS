using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorApplication _doctorApplication;

        public DoctorController(IDoctorApplication doctorApplication)
        {
            _doctorApplication = doctorApplication;
        }
        public IActionResult Index()
        {
            return View();
        }

        //public async Task<JsonResult> GetDoctorInfoListAsync()
        //{
        //    return Json(await _doctorApplication.)
        //}

    }
}
