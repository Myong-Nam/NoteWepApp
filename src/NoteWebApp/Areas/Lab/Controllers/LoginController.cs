using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
    public class LoginController : Controller
    {
        // GET: Lab/Login
        public ActionResult Index()
        {
            return View();
        }
		public ActionResult register()
		{
			return View();
		}
		public ActionResult SHAencoding(string password)
		{
			if (string.IsNullOrEmpty(password))
			{
				return View();
			} else
			{
				string salt = "1";

				string saltedPasword = salt + password;

				SHA256Managed sha256Managed = new SHA256Managed();

				ViewBag.Password = Convert.ToBase64String(sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(saltedPasword)));
			}

			

			

			return View();
		}
	}
}