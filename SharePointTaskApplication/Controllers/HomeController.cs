using Microsoft.Ajax.Utilities;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SharePointTaskApplication.Controllers
{
    public class AdminPage
    {
        public List<PersonData> persons { get; set; }
    }
    public class PersonApprove
    {
        public int Id { get; set; }
        public bool Approval { get; set; }
    }
    public class DemoDataProvider : IDataProvider
    {
        public void ChangeApproveStatus(int personId, bool approvalStatus)
        {
            //throw new NotImplementedException();
        }

        public void DeletePerson(int personId)
        {
            //throw new NotImplementedException();
        }

        public List<PersonData> GetData()
        {
            return new List<PersonData> { 
                new PersonData { Name = "Ann", Surname = "ererere", Email = "fdfddf@ngnhn.sws" },
                new PersonData { Name = "Ann2", Surname = "ererere2", Email = "fdfddf2@ngnhn.sws" },
            };

        }

        public void UpdatePerson(PersonData person)
        {

        }
    }
    public class DataProvider : IDataProvider
    {
        private string _url;

        public DataProvider(string url)
        {
            this._url = url;
        }
        public void ChangeApproveStatus(int personId, bool approvalStatus)
        {
            using (ClientContext clientContext = new ClientContext(_url))
            {
                var username = "User";
                var password = "Admin@2019";
                clientContext.Credentials = new NetworkCredential(username, password);
                Web web = clientContext.Web;
                List personList = web.Lists.GetByTitle("RegistrationData");
                ListItem listItem = personList.GetItemById(personId);
                listItem["ApprovalStatus"] = true;
                listItem.Update();
                clientContext.ExecuteQuery();
            }
        }
        public void DeletePerson(int personId)
        {
            using (ClientContext clientContext = new ClientContext(_url))
            {
                var username = "User";
                var password = "Admin@2019";
                clientContext.Credentials = new NetworkCredential(username, password);
                Web web = clientContext.Web;
                List personList = web.Lists.GetByTitle("RegistrationData");
                ListItem listItem = personList.GetItemById(personId);
                listItem.DeleteObject();
                clientContext.ExecuteQuery();
            }
        }
        public List<PersonData> GetData()
        {

            var _people = new List<PersonData>();
            DateTime date;
            using (ClientContext clientContext = new ClientContext(_url))
            {
                var username = "User";
                var password = "Admin@2019";
                clientContext.Credentials = new NetworkCredential(username, password);
                Web web = clientContext.Web;
                List personList = web.Lists.GetByTitle("RegistrationData");
                if (personList == null)
                {
                    ListCreationInformation creationInformation = new ListCreationInformation();
                    creationInformation.Title = "RegistrationData";
                    creationInformation.TemplateType = (int)ListTemplateType.Announcements;
                    personList = web.Lists.Add(creationInformation);
                    personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Surname'/>", true, AddFieldOptions.AddToAllContentTypes);
                    personList.Update();
                    personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='MiddleName'/>", true, AddFieldOptions.AddToAllContentTypes);
                    personList.Update();
                    personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Email'/>", true, AddFieldOptions.AddToAllContentTypes);
                    personList.Update();
                    personList.Fields.AddFieldAsXml("<Field Type='DateTime' DisplayName='DateOfBirth'/>", true, AddFieldOptions.AddToAllContentTypes);
                    personList.Update();
                    personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Sex'/>", true, AddFieldOptions.AddToAllContentTypes);
                    personList.Update();
                    personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Employer'/>", true, AddFieldOptions.AddToAllContentTypes);
                    personList.Update();
                    personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Position'/>", true, AddFieldOptions.AddToAllContentTypes);
                    personList.Update();
                    personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Country'/>", true, AddFieldOptions.AddToAllContentTypes);
                    personList.Update();
                    personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='City'/>", true, AddFieldOptions.AddToAllContentTypes);
                    personList.Update();
                    personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='ApprovalStatus'/>", true, AddFieldOptions.AddToAllContentTypes);
                    personList.Update();
                    clientContext.ExecuteQuery();
                }

                CamlQuery query = new CamlQuery();
                query.ViewXml = "<View/>";
                ListItemCollection items = personList.GetItems(query);

                //clientContext.Load(personList);
                clientContext.Load(items);
                // clientContext.Load(personList.Fields); //TODO разобрать

                clientContext.ExecuteQuery();

                //if(personList.Fields!=null)
                //    foreach(var field in personList.Fields)
                //    {
                //        person.Name =field.InternalName["Email"])
                //        { }
                //    }

                if (items != null)
                {
                    foreach (ListItem item in items)
                    {
                        foreach (KeyValuePair<string, object> pos in item.FieldValues)
                        {
                            var person = new PersonData();
                            person.Email = item["Email"].ToString();
                            if (!_people.Any(x => x.Email.Contains(person.Email)))
                            {
                                person.Id = (int)item["ID"];
                                person.Name = item["Title"].ToString();
                                person.Surname = item["Surname0"].ToString();
                                if (pos.Key == "MiddleName" && pos.Value != null)
                                    person.MiddleName = item["MiddleName"].ToString();
                                else person.MiddleName = "not defined";

                                //person.Email = item["Email"].ToString();

                                if (pos.Key == "DateOfBirth" && pos.Value != null)
                                {
                                    date = person.DateOfBirth;
                                    DateTime.TryParse(item["DateOfBirth"].ToString(), out date);  //TODO incorrect conversion
                                }
                                else person.DateOfBirth = default;

                                if (pos.Key == "Sex" && pos.Value != null)
                                    person.Sex = item["Sex"].ToString();
                                else person.Sex = "not defined";

                                if (pos.Key == "Employer" && pos.Value != null)
                                    person.Employer = item["Employer"].ToString();
                                else person.Employer = "not defined";

                                if (pos.Key == "Position" && pos.Value != null)
                                    person.Position = item["Position"].ToString();
                                else person.Position = "not defined";

                                if (pos.Key == "Country" && pos.Value != null)
                                    person.Country = item["Country"].ToString();
                                else person.Country = "not defined";

                                if (pos.Key == "City" && pos.Value != null)
                                    person.City = item["City"].ToString();
                                else person.City = "not defined";
                                //_people.Add(person);

                                if (pos.Key == "ApprovalStatus" && pos.Value != null)
                                {
                                    person.Approval = (bool)item["ApprovalStatus"];
                                    person.NewPerson = false;
                                }
                                else
                                {
                                    person.Approval = default;
                                    person.NewPerson = true;
                                }
                                _people.Add(person);
                            }
                        }
                    }
                }
            }
            return _people;
        }

        public void UpdatePerson(PersonData person)
        {
            using (ClientContext clientContext = new ClientContext(_url))//TODO create another method
            {
                Web web = clientContext.Web;
                List list = web.Lists.GetByTitle("UserRegistrationData");
                ListItemCreationInformation listItem = new ListItemCreationInformation();
                ListItem newItem = list.AddItem(listItem);
                newItem["Name"] = person.Name;
                newItem["Surname"] = person.Surname;
                newItem["Middle name"] = person.MiddleName;
                newItem["Email"] = person.Email;
                newItem["Date of birth"] = person.DateOfBirth;
                newItem["Sex"] = person.Sex;
                newItem["Employer"] = person.Employer;
                newItem["Position"] = person.Position;
                newItem["Country"] = person.Country;
                newItem["City"] = person.City;
                newItem["Approval Status"] = null;
                newItem.Update();
                clientContext.ExecuteQuery();
            }
        }
    }
    public class HomeController : Controller
    {
        //PersonData person = new PersonData();
        MailSender mailSender = new MailSender();
        Config config = new Config();

        //PersonContext personDb = new PersonContext();
        //private List<string> columns = new List<string>();
        //private string _url;

        //private List<PersonData> _people = new List<PersonData>();
        //private List<PersonData> _approvedPerson = new List<PersonData>();
        //private List<PersonData> _cancelledPerson = new List<PersonData>();

        private IDataProvider dataProvider;

        public HomeController()
        {
            //_url = config.SharePointUrl;
            //dataProvider = new DataProvider(config.SharePointUrl);
            dataProvider = new DemoDataProvider();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            //ClientContext clientContext = new ClientContext(_url);
            //var username = "User";
            //var password = "Admin@2019";
            //clientContext.Credentials = new NetworkCredential(username, password);
            //Web web = clientContext.Web;
            //List personList = web.Lists.GetByTitle("RegistrationData");
            //personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Surname'/>", true, AddFieldOptions.AddToAllContentTypes);
            //personList.Update();
            //personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='MiddleName'/>", true, AddFieldOptions.AddToAllContentTypes);
            //personList.Update();
            //personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Email'/>", true, AddFieldOptions.AddToAllContentTypes);
            //personList.Update();
            //personList.Fields.AddFieldAsXml("<Field Type='DateTime' DisplayName='DateOfBirth'/>", true, AddFieldOptions.AddToAllContentTypes);
            //personList.Update();
            //personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Sex'/>", true, AddFieldOptions.AddToAllContentTypes);
            //personList.Update();
            //personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Employer'/>", true, AddFieldOptions.AddToAllContentTypes);
            //personList.Update();
            //personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Position'/>", true, AddFieldOptions.AddToAllContentTypes);
            //personList.Update();
            //personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='Country'/>", true, AddFieldOptions.AddToAllContentTypes);
            //personList.Update();
            //personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='City'/>", true, AddFieldOptions.AddToAllContentTypes);
            //personList.Update();
            //personList.Fields.AddFieldAsXml("<Field Type='Text' DisplayName='ApprovalStatus'/>", true, AddFieldOptions.AddToAllContentTypes);
            //personList.Update();
            //clientContext.ExecuteQuery();
            //ViewBag.Message = "Добро пожаловать на сайт!";
            return View();
        }
        [HttpGet]
        //[Authorize(Roles ="Admin")]
        public ActionResult AdminPage()
        {
            var people = dataProvider.GetData();
            return View(people);
        }

        [HttpPost]
        //[Authorize(Roles ="Admin")]
        
        public ActionResult AdminPagePost(/*, bool[] approve*/)
        {
            List<PersonApprove> persons = null;
            using (StreamReader reader=new StreamReader(HttpContext.Request.InputStream))
            {
               var result= reader.ReadToEnd();
                persons = JsonConvert.DeserializeObject<List<PersonApprove>>(result);
            }
         
            if (persons == null)
                return View();
            {
                //return View(error); //ошибка
            }
                var approvedPerson = persons.Where(x => x.Approval == true).ToList();
                var cancelledPerson = persons.Where(x => x.Approval == false).ToList();
                foreach (var person in approvedPerson)
                {
                    if (person.NewPerson)
                    {
                        var email = person.Email;
                        var userPassword = Guid.NewGuid().ToString();
                        var passwordHash = userPassword/*.GetHashCode()*/;
                    mailSender.AcceptSender(email, userPassword);

                    dataProvider.ChangeApproveStatus(person.Id, true);

                    using (UserContext userDb = new UserContext())
                        {
                            var user = userDb.Users.FirstOrDefault(x => x.Email == person.Email);
                            if (user != null)
                            {
                                user.PasswordHash = passwordHash;
                                userDb.Entry(user).State = EntityState.Modified;
                            }
                        }

                        FormsAuthentication.SetAuthCookie(person.Email, true);
                        Response.Write("Изменения сохранены");
                    }
                }
                foreach (var person in cancelledPerson)
                {
                    var email = person.Email;
                    using (UserContext userDb = new UserContext())
                    {
                        var user = userDb.Users.FirstOrDefault(x => x.Email == person.Email);
                        if (user != null)
                        {
                            userDb.Entry(user).State = EntityState.Deleted;
                        }
                    }

                    FormsAuthentication.SetAuthCookie(email, true);
                    Response.Write("Изменения сохранены");
                dataProvider.DeletePerson(person.Id);
                mailSender.CancelSender(email);
            }
            return View();
        }

        //[HttpPost]
        //public ActionResult AuthorizeUser(PersonData person)
        //{
        //    //var email = person.Email.ToString();
        //    // var passwordHash = Guid.NewGuid().ToString().GetHashCode();

        //    if (ModelState.IsValid)
        //    {
        //        UserData user = null;
        //        using (UserContext userDb = new UserContext())
        //        {
        //            user = userDb.Users.FirstOrDefault(x => x.Email == person.Email);
        //        }
        //        if (user == null)
        //        {
        //            using (UserContext userDb = new UserContext())
        //            {
        //                userDb.Users.Add(new UserData { Email = person.Email, RoleId = 2 });
        //                userDb.SaveChanges();
        //                user = userDb.Users.Where(x => x.Email == person.Email).FirstOrDefault();
        //            }
        //            if (user != null)
        //            {
        //                FormsAuthentication.SetAuthCookie(person.Email, true);
        //                ModelState.AddModelError("", "Спасибо за регистрацию! В ближайшее время вам придет письмо с паролем");
        //                return RedirectToAction("Index", "Home");
        //            }

        //            dataProvider.UpdatePerson(person);
        //        }
        //        else ModelState.AddModelError("", "Пользователь с таким почтовым адресом уже зарегистрирован в системе");
        //    }
        //    return View();
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}