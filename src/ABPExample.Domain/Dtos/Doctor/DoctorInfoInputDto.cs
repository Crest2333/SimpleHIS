using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace HIS.Domain.Dtos.Doctor
{
    public class DoctorInfoInputDto
    {
        public int UserId { get; set; }

        public string ImgUrl { get; set; }

        public string Introduce { get; set; }

        public IFormFile ImgFile { get; set; }

    }
}
