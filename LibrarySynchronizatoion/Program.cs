using LibrarySynchronization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySynchronization
{
    class Program
    {
        static void Main(string[] args)
        {
            //LibraryCreator library = new LibraryCreator();
            //library.CreateLibrary();
            DocumentSynchronization docSync = new DocumentSynchronization();
            List<LibraryData> docsSP = docSync.GetDocumentsFromSP();
            List<LibraryData> docsDB = docSync.GetDataFromDB();

            DataBaseCorrector dataBaseCorrector = new DataBaseCorrector();
            dataBaseCorrector.CompareDocuments(docsSP, docsDB);
            dataBaseCorrector.CorrectDB();
        }
    }
}
