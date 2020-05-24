using Microsoft.Ajax.Utilities;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SharePointTaskApplication.Controllers
{
    public class HomeController : Controller
    {
        PersonData person = new PersonData();
        MailSender mailSender = new MailSender();
        Config config = new Config();

        //PersonContext personDb = new PersonContext();
        private List<string> columns = new List<string>();
        private string _url;

        private List<PersonData> _people = new List<PersonData>();
        private List<PersonData> _approvedPerson = new List<PersonData>();
        private List<PersonData> _cancelledPerson = new List<PersonData>();

        public HomeController()
        {
            _url = config.SharePointUrl;
        }

        UserData user = null;
        private string userPassword;
        public int passwordHash;

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
            return View(_people);
        }

        [HttpPost]
        //[Authorize(Roles ="Admin")]
        public ActionResult AdminPage(List<PersonData> persons/*, bool[] approve*/)
        {
            if (persons != null)
            {
                _approvedPerson = persons.Where(x => x.Approval == true).ToList();
                _cancelledPerson = persons.Where(x => x.Approval == false).ToList();
                foreach (var person in _approvedPerson)
                {
                    if (person.NewPerson)
                    {
                        var email = person.Email;
                        userPassword = Guid.NewGuid().ToString();
                        passwordHash = userPassword.GetHashCode();

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

                        using (ClientContext clientContext = new ClientContext(_url))
                        {
                            var username = "User";
                            var password = "Admin@2019";
                            clientContext.Credentials = new NetworkCredential(username, password);
                            Web web = clientContext.Web;
                            List personList = web.Lists.GetByTitle("RegistrationData");
                            ListItem listItem = personList.GetItemById(person.Id);
                            listItem["ApprovalStatus"] = true;
                            listItem.Update();
                            clientContext.ExecuteQuery();
                            mailSender.AcceptSender(email, userPassword);
                        }
                    }
                }
                foreach (var person in _cancelledPerson)
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

                    using (ClientContext clientContext = new ClientContext(_url))
                    {
                        var username = "User";
                        var password = "Admin@2019";
                        clientContext.Credentials = new NetworkCredential(username, password);
                        Web web = clientContext.Web;
                        List personList = web.Lists.GetByTitle("RegistrationData");
                        ListItem listItem = personList.GetItemById(person.Id);
                        listItem.DeleteObject();
                        clientContext.ExecuteQuery();
                        mailSender.CancelSender(email);
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult AuthorizeUser(PersonData person)
        {
            //var email = person.Email.ToString();
            // var passwordHash = Guid.NewGuid().ToString().GetHashCode();

            if (ModelState.IsValid)
            {
                UserData user = null;
                using (UserContext userDb = new UserContext())
                {
                    user = userDb.Users.FirstOrDefault(x => x.Email == person.Email);
                }
                if (user == null)
                {
                    using (UserContext userDb = new UserContext())
                    {
                        userDb.Users.Add(new UserData { Email = person.Email, RoleId = 2 });
                        userDb.SaveChanges();
                        user = userDb.Users.Where(x => x.Email == person.Email).FirstOrDefault();
                    }
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(person.Email, true);
                        ModelState.AddModelError("", "Спасибо за регистрацию! В ближайшее время вам придет письмо с паролем");
                        return RedirectToAction("Index", "Home");
                    }

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
                else ModelState.AddModelError("", "Пользователь с таким почтовым адресом уже зарегистрирован в системе");
            }
            return View();
        }

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