using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySynchronization
{
    static class DataLibrarySPConnector
    {
        public static ListItemCollection GetDataLibraryConnection(string url)
        {
            using (ClientContext clientContext = new ClientContext(url))
            {
                var username = "User";
                var password = "Admin@2019";
                clientContext.Credentials = new NetworkCredential(username, password);

                Web web = clientContext.Web;
                List documentsList = web.Lists.GetByTitle("CommonDocuments");
                CamlQuery query = new CamlQuery();
                query.ViewXml = "<View/>";
                ListItemCollection listItems = documentsList.GetItems(query);

                clientContext.Load(documentsList);
                clientContext.Load(listItems);
                clientContext.ExecuteQuery();

                return listItems;
            }
        }
    }
}
