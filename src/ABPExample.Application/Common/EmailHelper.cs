using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace HIS.Application.Common
{
    public class EmailHelper
    {
        /// <summary>
        /// 发送Email
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static void Send(string toAddress, string title, string content)
        {
            MailMessage mailMsg = new MailMessage();//实例化对象
            mailMsg.From = new MailAddress("949343937@qq.com", "测试");//源邮件地址和发件人
            mailMsg.To.Add(new MailAddress(toAddress));//收件人地址
            mailMsg.Subject = title;//发送邮件的标题
            mailMsg.Body = content;//发送邮件的内容
            //指定smtp服务地址（根据发件人邮箱指定对应SMTP服务器地址）
            SmtpClient client = new SmtpClient();//格式：smtp.126.com  smtp.164.com
            client.Host = "smtp.qq.com";
            //要用587端口
            client.Port = 587;//端口
            //加密
            client.EnableSsl = true;
            //通过用户名和密码验证发件人身份
            client.Credentials = new NetworkCredential("949343937@qq.com", "qhquvkzeduxgbdea"); // 
            //发送邮件
            try
            {
                client.Send(mailMsg);
            }
            catch (SmtpException ex)
            {

            }
            Console.WriteLine("邮件已发送，请注意查收！");
        }


    }
}
