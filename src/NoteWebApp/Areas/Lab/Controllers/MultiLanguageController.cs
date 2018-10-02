using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
    public class MultiLanguageController : Controller
    {
        // GET: Lab/MultiLanguage
        public ActionResult Index()
        {
			CultureInfo koKR = new CultureInfo("ko-KR");
			CultureInfo enUS = new CultureInfo("en-US");

			//Thread.CurrentThread.CurrentCulture = enUS;	// CurrentCulture는 날짜 포맷 소수점 자리수 등의 정보와 관련이 있다.
			Thread.CurrentThread.CurrentUICulture = koKR;   // label 처리만 할 것이라면 CurrentUICulture 만으로 충분하다.

			ViewBag.lang = Resources.Label.Hello;

			return View();
        }
    }
}