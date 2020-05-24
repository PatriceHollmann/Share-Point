using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SharePointTaskApplication
{
    public class Country
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int PersonId { get; set; }
        public PersonData personData { get; set; }
    }
}