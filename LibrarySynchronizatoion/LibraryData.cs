using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySynchronization
{
    class LibraryData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateUpdate { get; set; }
        public byte [] Docs { get; set; }
    }
}
