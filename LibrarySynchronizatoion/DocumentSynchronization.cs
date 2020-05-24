using LibrarySynchronization;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySynchronization
{
    class DocumentSynchronization
    {
        private string _url;
        Config config = new Config();
        public List<LibraryData> DocsSP { get; set; }
        //public Dictionary<string,DateTime> DocsSP { get; set; }
        public List<LibraryData> DocsDB { get; set; }
        //public List<string> DocsToAdd { get; set; }
        //public List<string> DocsToDel { get; set; }
        public DocumentSynchronization()
        {
            _url = config.SharePointUrl;
            DocsSP = new List<LibraryData>();
            DocsDB = new List<LibraryData>();
            //DocsToAdd = new List<string>();
            //DocsToDel = new List<string>();
        }
       public List<LibraryData> GetDocumentsFromSP()
        {
            var listItems = DataLibrarySPConnector.GetDataLibraryConnection(_url);
            //using (ClientContext clientContext = new ClientContext(_url))
            //{
            //    var username = "User";
            //    var password = "Admin@2019";
            //    clientContext.Credentials = new NetworkCredential(username, password);

            //    Web web = clientContext.Web;
            //    List documentsList = web.Lists.GetByTitle("CommonDocuments");
            //    CamlQuery query = new CamlQuery();
            //    query.ViewXml = "<View/>";
            //    ListItemCollection listItems = documentsList.GetItems(query);

            //    clientContext.Load(documentsList);
            //    clientContext.Load(listItems);
            //    clientContext.ExecuteQuery();
                if (listItems != null)
                {
                    foreach (var item in listItems)
                    {
                        var name = item.FieldValues["FileLeafRef"].ToString();
                        // var date = item.FieldValues["Modified"];
                        //date = person.DateOfBirth;
                        DateTime.TryParse(item.FieldValues["Modified"].ToString(), out var date);
                        DocsSP.Add(new LibraryData { Name = name, DateUpdate = date });
                    }
                }
            return DocsSP;
        }

        public List<LibraryData> GetDataFromDB()
        {
            using (LibraryContext libraryContext = new LibraryContext())
            {
                foreach (var item in libraryContext.Documents)
                {
                    var name = libraryContext.Documents.Select(x => x.Name).FirstOrDefault();
                    var date = libraryContext.Documents.Select(x => x.DateUpdate).FirstOrDefault();
                    DocsDB.Add(new LibraryData { Name = name, DateUpdate = date });
                }
            }
            return DocsDB;
        }
    }
}
