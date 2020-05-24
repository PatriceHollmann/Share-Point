using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SharePointTaskApplication
{
    public class MailChecker
    {
        public List<string> Errors { get; set; }
        public MailChecker()
        {
            Errors = new List<string>();
        }
        public void MailParamsCheck(string server, string email)
        {
            //string emailPattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            //    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

            if (String.IsNullOrEmpty(server) || String.IsNullOrEmpty(email))
            {
                string errorMessage = "Не полные исходные данные для отправки почты";
                Errors.Add(errorMessage);
            }
            //else
            //{
            //    if (!Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
            //    {
            //        string errorMessage = "Не корректно введен адрес получателя почты";
            //        Errors.Add(errorMessage);
            //    }
            //}
        }
    }
}