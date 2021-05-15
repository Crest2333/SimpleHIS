using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HIS.Application.Interface;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace HIS.Application.Application
{
    public class CommonApplication:ICommonApplication,ITransientDependency
    {

        public CommonApplication()
        {
        }
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }
            string webRootPath =Directory.GetCurrentDirectory(); 
            string uploadPath = Path.Combine("Images", DateTime.Now.ToString("yyyyMMdd"));
            string dirPath = Path.Combine(webRootPath, uploadPath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string fileExt = Path.GetExtension(file.FileName).Trim('.'); //文件扩展名，不含“.”
            string newFileName = Guid.NewGuid().ToString().Replace("-", "") + "." + fileExt; //随机生成新的文件名
            var filePath = Path.Combine(dirPath, newFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var url = $@"\{uploadPath}\{newFileName}";
            return url;
        }

        public async Task BatchSendEmail(List<string> emailList,string message)
        {
            foreach (var item in emailList)
            {
                //await _emailSender.SendAsync("949343937@qq.com", item, message);

            }
        }
    }
}
