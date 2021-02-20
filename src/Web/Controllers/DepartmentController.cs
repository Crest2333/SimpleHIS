using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABPExample.Application.Interface;
using ABPExample.Domain.Dtos.Department;
using ABPExample.Domain.Dtos.Doctor;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentApplication _departmentApplication;

        public DepartmentController(IDepartmentApplication departmentApplication,IDoctorApplication doctorApplication)
        {
            _departmentApplication = departmentApplication;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DerpatmentDocList(int departmentId)
        {
            ViewBag.DepartmentId = departmentId;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetDepartmentList(DepartmentSearchDto param)
        {
            return Json(await _departmentApplication.List(param));
        }

        public async Task<JsonResult> GetAllDepartment()
        {
            return Json(await _departmentApplication.GetAllDepartment());
        }

        public async Task<JsonResult> Add(AddDepartmentInputDto inputDto)
        {
            return Json(await _departmentApplication.Add(inputDto));
        }

        public async Task<JsonResult> AddDepartmentPersonnel(AddPersonnelInputDto inputDto)
        {
            return Json(await _departmentApplication.AddDepartmentPersonnel(inputDto));
        }

        public async Task<JsonResult> BatchAdd(List<AddDepartmentInputDto> inputDtoList)
        {
            return Json(await _departmentApplication.BatchAdd(inputDtoList));
        }

        public async Task<JsonResult> BatchAddDepartmentPersonnel(List<AddPersonnelInputDto> inputDtoList)
        {
            return Json(await _departmentApplication.BatchAddDepartmentPersonnel(inputDtoList));
        }

        [HttpPost]
        public async Task<JsonResult> BatchDelete(List<int> idList)
        {
            return Json(await _departmentApplication.BatchDelete(idList));
        }

        public async Task<JsonResult> BatchRemovePersonnel(List<int> idList)
        {
            return Json(await _departmentApplication.BatchRemovePersonnel(idList));
        }

        public async Task<JsonResult> Delete(long id)
        {
            return Json(await _departmentApplication.Delete(id));
        }

        [HttpPost]

        public async Task<JsonResult> GetDepartmentDocList(GetDoctorInputDto param)
        {
            return Json(await _departmentApplication.GetDoctorInfoAsync(param));
        }

        [HttpGet]
        public async Task<JsonResult> GetDoctorListByDepartmentId(int departmentId)
        {
            return Json(await _departmentApplication.GetDoctorInfoAsync(new GetDoctorInputDto
            {
                DepartmentId = departmentId,
                PageIndex = 1,
                PageSize = 10000
            }));
        }
    }
}
