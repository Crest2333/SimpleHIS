using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HIS.Application.Interface
{
    public interface ICommonApplication
    {
        Task<string> UploadFileAsync(IFormFile file);

        Task BatchSendEmail(List<string> emailList, string message);
    }
}
