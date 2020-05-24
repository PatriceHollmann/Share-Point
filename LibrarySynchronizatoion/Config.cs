using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LibrarySynchronization
{
    public class Config
    {
        public string SharePointUrl { get; set; }
        public string DocFolder { get; set; }

        public Config()
        {
            SharePointUrl = ConfigurationManager.AppSettings["SharePointUrl"];
            DocFolder = ConfigurationManager.AppSettings["docFolder"];
        }
    }
}