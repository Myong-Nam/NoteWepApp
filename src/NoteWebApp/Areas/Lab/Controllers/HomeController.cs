using NoteWebApp.Areas.Lab.Models;
using NoteWebApp.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
    public class HomeController : Controller
    {
        // GET: Lab/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}