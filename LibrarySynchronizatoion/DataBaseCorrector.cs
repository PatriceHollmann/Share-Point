using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySynchronization
{
    class DataBaseCorrector
    {
        DocumentSynchronization documentSynchronization = new DocumentSynchronization();
        private List<LibraryData> _docsToAdd = new List<LibraryData>();
        private List<LibraryData> _docsToDel = new List<LibraryData>();

        private string _url;
        Config config = new Config();

        public DataBaseCorrector()
        {
            _url = config.SharePointUrl;
        }
        public void CompareDocuments(List<LibraryData> docsSP, List<LibraryData> docsDB)
        {
            foreach (var doc in docsSP)
            {
                if (!docsDB.Any(x => x.Equals(doc)))
                {
                    _docsToAdd.Add(doc);
                }
            }
            foreach (var doc in docsDB)
            {
                if (!docsSP.Any(x => x.Equals(doc)))
                {
                    _docsToDel.Add(doc);
                }
            }
        }
        public /*Stream*/ void CorrectDB()
        {
            if(_docsToDel!=null)
            {
                using (LibraryContext libraryContext = new LibraryContext())
                {
                    foreach (var item in _docsToDel)
                    {
                        libraryContext.Entry(item).State = EntityState.Deleted;
                    }
                }
            }
            if (_docsToAdd != null)
            {
                var listItems = DataLibrarySPConnector.GetDataLibraryConnection(_url);
                foreach (var doc in _docsToAdd)
                {
                    foreach (var item in listItems)
                    {
                        var docName = _docsToAdd.Select(x => x.Name).FirstOrDefault();

                        using (ClientContext clientContext = new ClientContext(_url))
                        {
                            var username = "User";
                            var password = "Admin@2019";
                            clientContext.Credentials = new NetworkCredential(username, password);
                            FileInformation fInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(clientContext, item.FieldValues["FileRef"].ToString());
                            using (LibraryContext libraryContext = new LibraryContext())
                            {
                                using (FileStream fileStream = new FileStream(fInfo.Stream.ToString(), FileMode.Open)) //TODO correct path
                                {
                                    LibraryData libraryData = new LibraryData();
                                    libraryData.Docs = new byte[fileStream.Length];
                                    fileStream.Read(libraryData.Docs, 0, (int)fileStream.Length);
                                    libraryContext.Documents.Add(libraryData);
                                    libraryContext.SaveChanges();
                                } 
                            }
                            //return fInfo.Stream;
                            //var name = item.FieldValues[docName];
                            //var name = item.FieldValues["FileLeafRef"].ToString();
                        }
                    }

                }
            }
            //return null;
        }
    }
}
