using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSales.ViewModels;
using OnlineSales.Models;
using System.Net.Mail;
using System.Web.Security;

namespace OnlineSales.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private BiccTyresEntities bicctyre;

        private static Random rnd;

        public UserController()
        {
            bicctyre = new BiccTyresEntities();
            rnd = new Random();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel objChangePassword)
        {
            string pwd = string.Empty;
            var user = bicctyre.Accounts.Where(x => x.UserName == objChangePassword.UserName).FirstOrDefault();
           
            if (user.UserName != null)
            {
                int id = user.AccountID;
                if (string.Compare(crypto.Hash(objChangePassword.OldPassword), user.Password) == 0)
                {
                    if (objChangePassword.NewPassword == objChangePassword.ConfirmPassword)
                    {
                        var data = bicctyre.Accounts.Where(x => x.AccountID == id).FirstOrDefault();
                        data.Password = crypto.Hash(objChangePassword.NewPassword);
                        string name = user.FirstName;
                        string surname = user.LastName;
                        pwd = objChangePassword.NewPassword;
                        bicctyre.Entry(data).State = System.Data.Entity.EntityState.Modified;
                        bicctyre.SaveChanges();
                        SendUsEmail(name, surname, objChangePassword.UserName, pwd);
                        return RedirectToAction("Login", "User");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "New Password must equal to Confirm Password");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Old Password is Incorrect");
                    return View();
                }
                
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This Email address does not exist");
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserViewModel objUserViewModel)
        {
            var user = bicctyre.Accounts.Where(x => x.UserName == objUserViewModel.UserName).FirstOrDefault();
            if (objUserViewModel.UserName != null && objUserViewModel.Password != null)
            {
                if (string.Compare(crypto.Hash(objUserViewModel.Password), user.Password) == 0)
                {
                    Session["role"] = user.Role;
                    Session["username"] = user.FirstName + " " + user.LastName;
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    return RedirectToAction("Index", "Item");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Your Password is incorrect");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email address or Password does not exist");
                return View();
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Account accView)
        {
            int n = 10;
            string pwd1 = "";
            char[] password = "acBbYyWwKkLlmNMOpP21983JkTdD0".ToArray();
            for (int i = 0; i < n - 1; i++)
            {
                pwd1 = pwd1 + password[rnd.Next(0, password.Length)];
            }
            string pwd = pwd1;

            accView.Role = "user";
            accView.Password = crypto.Hash(pwd);
            accView.IsActive = true;
            accView.CreatedOn = DateTime.Now.ToShortDateString();
            bicctyre.Accounts.Add(accView);
            bicctyre.SaveChanges();
            SendUsEmail(accView.FirstName, accView.LastName, accView.UserName, pwd);
            return RedirectToAction("Index","Item");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(UserViewModel objUserViewModel)
        {
            var user = bicctyre.Accounts.Where(x => x.UserName == objUserViewModel.UserName).FirstOrDefault();
            if (user.UserName != null)
            {
                int n = 10;
                string pwd1 = "";
                char[] password = "acBbYyWwKkLlmNMOpP21983JkTdD0".ToArray();
                for (int i = 0; i < n - 1; i++)
                {
                    pwd1 = pwd1 + password[rnd.Next(0, password.Length)];
                }
                string pwd = pwd1;
                int id = user.AccountID;
                var data = bicctyre.Accounts.Where(x => x.AccountID == id).FirstOrDefault();
                data.Password = crypto.Hash(pwd);
                string name = user.FirstName;
                string surname = user.LastName;
                bicctyre.Entry(data).State = System.Data.Entity.EntityState.Modified;
                bicctyre.SaveChanges();
                SendUsEmail(name, surname, objUserViewModel.UserName, pwd);
                return RedirectToAction("Login","User");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "This Email address does not exist!!!");
                return View();
            }
        }

        [NonAction]
        public void SendUsEmail(string fname, string lname, string emailID, string password)
        {
            var account = "kimsfood20@gmail.com";
            var accountTitle = "Bicc Tyres Shop";
            var fromEmail = new MailAddress(account, accountTitle);
            var fromEmailPassword = "KimsKims2020";
            var toEmail = new MailAddress(emailID);

            string subject = "Your New Password for BiccTyres System" ;
            string body = "Hello Dear " + fname + " " + lname + "<br/><br/>Please find in this email content your password : <b>" + password +
                "</b> in order to get access to our Bicc Tyres and Wheels System ." + "<br/><br/>"+accountTitle;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

                smtp.Send(message);
        }

    }
}