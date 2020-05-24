using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SharePointTaskApplication
{
    public class Config
    {
        public string ServerFrom { get; set; }

        public string EmailFrom { get; set; }
        public string SharePointUrl { get; set; }

        public Config()
        {
            ServerFrom = ConfigurationManager.AppSettings["serverFrom"];
            EmailFrom = ConfigurationManager.AppSettings["emailFrom"];
            SharePointUrl = ConfigurationManager.AppSettings["SharePointUrl"];
        }
    }
}