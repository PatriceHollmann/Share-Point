using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SharePointTaskApplication
{
    public class MailSender
    {
        MailChecker mailChecker = new MailChecker();
        Config config = new Config();

        private string _serverFrom;
        private string _emailFrom;

        public List<string> Errors { get; set; }
        public MailSender()
        {
            _serverFrom = config.ServerFrom;
            _emailFrom = config.EmailFrom;

            Errors = new List<string>();
        }
        public void AcceptSender(string emailTo, string userPassword)
        {
            mailChecker.MailParamsCheck(_serverFrom, emailTo);
            MailAddress from = new MailAddress(_emailFrom);
            MailAddress to = new MailAddress(emailTo);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Регистрация на сайте";
            message.Body = "Спасибо за регистрацию! Ваш пароль для входа на сайт " + userPassword;
            SmtpClient client = new SmtpClient(_serverFrom);
            client.UseDefaultCredentials = true;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ошибка создания письма " + ex.ToString();
                Errors.Add(errorMessage);
            }
            Errors = Errors.Concat(mailChecker.Errors).ToList();
        }
        public void CancelSender(string emailTo)
        {
            mailChecker.MailParamsCheck(_serverFrom, emailTo);
            MailAddress from = new MailAddress(_emailFrom);
            MailAddress to = new MailAddress(emailTo);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Регистрация на сайте";
            message.Body = "К сожалению, Вам отказано в регистрации";
            SmtpClient client = new SmtpClient(_serverFrom);
            client.UseDefaultCredentials = true;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                string errorMessage = "Ошибка создания письма " + ex.ToString();
                Errors.Add(errorMessage);
            }
            Errors = Errors.Concat(mailChecker.Errors).ToList(); //TODO -  вывод??
        }
}
}