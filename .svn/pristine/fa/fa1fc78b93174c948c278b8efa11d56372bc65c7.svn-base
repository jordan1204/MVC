using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonResource.Models;
using System.Net.Mail;

namespace ebSuccessWebSite.Models
{
    public class TrialFormHandler : BaseHandler
    {
        public void SaveTrialForm(CustomerTrial_Form form)
        {
            if (form != null)
            {
                db.CustomerTrial_Form.Add(form);
                db.SaveChanges();
                SendToMarketingEmail(form);
            }
            else
            {
                throw new ArgumentNullException("Form data is null");
            }  
        }
        
        private void SendToMarketingEmail(CustomerTrial_Form form)
        {
            //設定smtp主機
            SmtpClient mySmtp = new SmtpClient("mail.ebsuccess.com");
            //設定smtp帳密
            mySmtp.Credentials = new System.Net.NetworkCredential("mkt", "70479912");
            //信件內容
            string pcontect = String.Format("姓名：{0}<br/>聯絡電話：{1}<br/>手機號碼：{2}<br/>E-mail：{3}<br/>客戶留言：{4}<br/>欲試用產品：{5}", form.Name, form.Telephone, form.Cellphone, form.Email, form.Message, form.ProductName);
            //設定mail內容
            MailMessage msgMail = new MailMessage();
            //寄件者
            msgMail.From = new MailAddress("marketing@ebsuccess.com");
            //收件者
            msgMail.To.Add("marketing@ebsuccess.com");
            //主旨
            msgMail.Subject = "線上客戶詢問單";
            //信件內容(含HTML時)
            AlternateView alt = AlternateView.CreateAlternateViewFromString(pcontect, null, "text/html");
            msgMail.AlternateViews.Add(alt);
            //寄mail
            mySmtp.Send(msgMail);
        }
    }
}