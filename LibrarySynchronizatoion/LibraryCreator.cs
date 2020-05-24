using Microsoft.SharePoint.Client;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace LibrarySynchronization
{
    class LibraryCreator
    {
        Config config = new Config();
        private string _url;
        public LibraryCreator()
        {
            _url = config.SharePointUrl;
        }
        public void CreateLibrary()
        {
            string[]filePathes=Directory.GetFiles(config.DocFolder);
            if (filePathes.Length > 0)
            {
                using (ClientContext clientContext = new ClientContext(_url))
                {
                    var username = "User";
                    var password = "Admin@2019";
                    clientContext.Credentials = new NetworkCredential(username, password);

                    Web web = clientContext.Web;
                    foreach (var filePath in filePathes)
                    {
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                        {
                            FileCreationInformation newFile = new FileCreationInformation();
                            newFile.ContentStream = fileStream;
                            newFile.Url = System.IO.Path.GetFileName(filePath);
                            newFile.Overwrite = true;
                            List docs = web.Lists.GetByTitle("CommonDocuments");
                            Microsoft.SharePoint.Client.File uploadFile = docs.RootFolder.Files.Add(newFile);
                            clientContext.Load(uploadFile);
                            clientContext.ExecuteQuery();
                        }
                    }
                }
            }
        }
    }
}
