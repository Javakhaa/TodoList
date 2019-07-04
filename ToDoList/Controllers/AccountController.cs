using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToDoList.Helper;
using ToDoList.Models;
using ToDoList.Models.Account;

namespace ToDoList.Controllers
{
    public class AccountController : Controller
    {
        DataBaseConnectionDataContext db = new DataBaseConnectionDataContext();
        public AccountController()
        {
            if (db == null)
            {
                db = new DataBaseConnectionDataContext();
            }
        }

        public ActionResult Registration()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationViewModel registrationViewModel)
        {
            //using (var db = new DataBaseConnectionDataContext())
            //{

            //}
            if (ModelState.IsValid)
            {
                var notConfirmedUser = new NotConfirmedUser()
                {
                    FirstName = registrationViewModel.FirstName,
                    LastName = registrationViewModel.LastName,
                    Email = registrationViewModel.Email,
                    Password = AccountHelper.GetHash256ByString(registrationViewModel.Password + AccountHelper.AuthSecret),
                    ConfirmationCode = AccountHelper.RandomString(),
                    CreateDate = DateTime.Now,
                    RequestIp = Request.UserHostAddress
                };
                db.NotConfirmedUsers.InsertOnSubmit(notConfirmedUser);
                db.SubmitChanges();

                ModelState.Clear();
            }
            return Content("თქვენ წარმატებით გაიარეთ რეგისტრაცია");
        }


        public ActionResult Confirmation(string id)
        {
            var notConfirmedUser = db.NotConfirmedUsers.FirstOrDefault(x => x.ConfirmationCode == id);
            db.Users.InsertOnSubmit(

                new User()
                {
                    FirstName = notConfirmedUser.FirstName,
                    LastName = notConfirmedUser.LastName,
                    Email = notConfirmedUser.Email,
                    Password = notConfirmedUser.Password,
                    RequestIp = notConfirmedUser.RequestIp,
                    CreateDate = DateTime.Now

                });
            db.SubmitChanges();
            db.NotConfirmedUsers.DeleteAllOnSubmit(db.NotConfirmedUsers.Where(x => x.Email == notConfirmedUser.Email));
            db.SubmitChanges();
            return RedirectToAction("Login");

        }

        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string password = AccountHelper.GetHash256ByString(loginViewModel.Password + AccountHelper.AuthSecret);
                var user = db.Users.FirstOrDefault(x => x.Email == loginViewModel.Email && x.Password == password);
                if (user == null)
                {
                    ViewBag.error = "მომხმარებელი მსგავსი მონაცემით ვერ მოიძებნა";
                    return View();
                }

                else
                {
                    Session["user"] = user;
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();

        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }


        public ActionResult Forgot()
        {
            return View();
        }

        [HttpPost] // aq gaigzavna konfirmaciis kodi meilze
        public ActionResult Forgot(string Email)
        {
            var std = db.Users.FirstOrDefault(s => s.Email == Email);
            if (std != null)
            {
                var ForgotPasswordView = new ForgetPsw()
                {
                    Email = Email,
                    Code = AccountHelper.RandomString()
                };

                db.ForgetPsws.InsertOnSubmit(ForgotPasswordView); //////fix
                db.SubmitChanges();
                ViewBag.message = "Confirmation Code Sent";
                return View();
            }
            else
            {
                ViewBag.message = "User Not Found!";
            }
            return View();
        }

        // ak damireke an aq damireke 
        public ActionResult ConfirmNewPassword(string Email, string Code)
        {
            ViewBag.message = "false";
            var std = db.ForgetPsws.Any(s => s.Email == Email && s.Code == Code);
            if (std)
            {
                Session["User"] = Email;
                ViewBag.message = "true";
                return View("ResetPassword");
            }

            return View();
        }

        [HttpPost]
        public ActionResult ConfirmNewPassword(ConfirmNewPasswordViewModel model)
        {

            string Email = Session["User"].ToString();
            var std = db.Users.FirstOrDefault(x => x.Email == Email);
            std.Password = AccountHelper.GetHash256ByString(model.Password);
            db.ForgetPsws.DeleteAllOnSubmit(db.ForgetPsws.Where(x => x.Email == Email));
            db.SubmitChanges();
            Session.Clear();
            return View("Login");
            //ViewBag.message = "false";
            //var std = db.ForgetPsws.FirstOrDefault(s => s.Email == Email && s.Code == Code);
            //if (std != null)
            //{
            //    Session["User"] = Email;
            //    ViewBag.message = "true";
            //    return View("ResetPassword");
            //}

            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost] // rato ar gadaagdo login ze maca
        public ActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {

            string Email = Session["User"].ToString();
            var std = db.Users.FirstOrDefault(x => x.Email == Email);
            std.Password = AccountHelper.GetHash256ByString(resetPasswordViewModel.Password);
            db.ForgetPsws.DeleteAllOnSubmit(db.ForgetPsws.Where(x => x.Email == Email));
            db.SubmitChanges();
            Session.Clear();
            return View("Login");
        }

    }
    public class ConfirmNewPasswordViewModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}



