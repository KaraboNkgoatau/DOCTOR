using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DOCTOR.Models;
using System.Security.Cryptography;
using System.Text;


namespace DOCTOR.Controllers
{
    public class DoctorController : Controller
    { 
        Homework11Database272Entities3 db = new Homework11Database272Entities3();

        string GenerateHashedPassword(string Password)
        {
            try
            {
                using (SHA256 ShaHash = SHA256.Create())
                {
                    byte[] byteArrayOfPassword = ShaHash.ComputeHash(Encoding.UTF8.GetBytes(Password));
                    StringBuilder PasswordHashed = new StringBuilder();

                    for (int Counter = 0; Counter < byteArrayOfPassword.Length; Counter++)
                    {
                        PasswordHashed.Append(byteArrayOfPassword[Counter].ToString("x2"));
                    }

                    return PasswordHashed.ToString();
                }
            }
            catch (Exception GeneralExceptioin)
            {
                ViewBag.Error = GeneralExceptioin.Message;
                return ViewBag.Error;
            }
        }
        // GET: Doctor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult doctorLogin(string Username,string Password, string SignIn,string User, string UserType)
        {
            {
                db.Configuration.ProxyCreationEnabled = false;
                try
                {
                    if (SignIn == "Sign In")
                    {
                        User CheckUser = db.Users.Where(X => X.Username == Username).FirstOrDefault();

                        if (CheckUser == null)
                        {
                            return View("CreateUser");
                        }
                        else
                        {
                            string HashedPassword = GenerateHashedPassword(Password);
                            User CheckUserPassword = db.Users.Where(Y => Y.Username == Username & Y.Password == HashedPassword).FirstOrDefault();

                            if (CheckUserPassword == null)// if password is incorrect, object is null
                            {
                                return View("CreateUser");
                            }

                            else
                            {
                                UserType CheckUserType = new UserType();
                                CheckUserType = db.UserTypes.Where(Z => Z.ID == CheckUser.UserTypeID).FirstOrDefault();

                                if (CheckUserType.Description == "Admin Staff")
                                {
                                    return RedirectToAction("Index", "Modules");
                                }
                                else
                                {

                                    return View("CreateUser", CheckUser);
                                }
                            }
                        }
                    }
                }
                catch (Exception GeneralExceptioin)
                {
                    ViewBag.Error = GeneralExceptioin.Message;
                    return View();
                }

                return View();
            }
            return View();
        }
        public ActionResult CreateUser(string Name, string Surname, string UserType, string Username, string Password, string ConfirmedPassword, string btnSubmit)
        {
            {
                if (btnSubmit == "Submit")
                {
                    if (Password != ConfirmedPassword)
                    {
                        ViewBag.PasswordMatchError = "Passwords do not match, please re-enter!";
                    }
                    else
                    {
                        User newUser = new User();
                        Student newStudent = new Student();

                        newStudent.Name = Name;
                        newStudent.Surname = Surname;

                        UserType CheckForUserType = db.UserTypes.Where(X => X.Description == UserType).FirstOrDefault();
                        if (CheckForUserType != null)
                        {
                            newUser.UserTypeID = CheckForUserType.ID;
                        }
                        newUser.Username = Username;
                        newUser.Password = GenerateHashedPassword(Password);
                        newStudent.UserID = newUser.ID;

                        db.Students.Add(newStudent);
                        db.Users.Add(newUser);
                        db.SaveChanges();
                        return View("Login");
                    }
                }
                return View();
            }
            
        }

    }
}