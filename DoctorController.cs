using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using DOCTOR.Models;
namespace DOCTOR.Controllers
{
    public class DoctorController : Controller
    {
        OnlinePharmacyEntities db = new OnlinePharmacyEntities();

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

        public ActionResult doctorLogin(string Username, string Password, string SignIn, string User, string UserType)
        {
            {
                db.Configuration.ProxyCreationEnabled = false;
                try
                {
                    if (SignIn == "Sign In")
                    {
                        User CheckUser = db.Users.Where(X => X.UserEmail == Username).FirstOrDefault();

                        if (CheckUser == null)
                        {
                            return View("CreateUser");
                        }
                        else
                        {
                            string HashedPassword = GenerateHashedPassword(Password);
                            User CheckUserPassword = db.Users.Where(Y => Y.UserEmail == Username & Y.UserPassword == HashedPassword).FirstOrDefault();

                            if (CheckUserPassword == null)// if password is incorrect, object is null
                            {
                                return View("CreateUser");
                            }

                            else
                            {
                                UserType CheckUserType = new UserType();
                                CheckUserType = db.UserTypes.Where(Z => Z.ID == CheckUser.UserType).FirstOrDefault();

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
                        Doctor newDoctor = new Doctor();

                        newDoctor.Doctor_Email = Name;
                        newDoctor.Doctor_Name = Surname;

                        UserType CheckForUserType = db.UserTypes.Where(X => X.Description == UserType).FirstOrDefault();
                        if (CheckForUserType != null)
                        {
                            newUser.UserType = CheckForUserType.ID;
                        }
                        newUser.UserEmail = Username;
                        newUser.UserPassword = GenerateHashedPassword(Password);
                        newDoctor.Doctor_ID = newUser.UserID;

                        db.Doctors.Add(newDoctor);
                        db.Users.Add(newUser);
                        db.SaveChanges();
                        return View("Login");
                    }
                }
                return View();
            }
        }
              public ActionResult Test()
        {
            return View();
        }

    }

    }
