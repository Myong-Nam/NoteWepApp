using NoteWebApp.Models;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoteWebApp
{
	public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

			//
			// Database setting
			//
			DbHelper.ConnectionString = ConfigurationManager.ConnectionStrings["MainDB"].ToString();

			CultureInfo cultureEN_US = new CultureInfo("en-US");
			CultureInfo cultureEN = new CultureInfo("en");
			CultureInfo cultureKO_KR = new CultureInfo("ko-KR");
			CultureInfo culturezh_CN = new CultureInfo("zh-CN");

			Thread.CurrentThread.CurrentCulture = cultureEN_US;
			Thread.CurrentThread.CurrentUICulture = cultureEN_US;
		}
	}
}
