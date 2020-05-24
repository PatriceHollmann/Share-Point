using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySynchronization
{
    class LibraryContext:DbContext
    {
        public LibraryContext():base("DefaultConnection")
        {

        }
        public DbSet<LibraryData>Documents { get; set; }
    }
}
