using NoteWebApp.Models;
using System.Configuration;
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
			//bHelper.Oracle.DbHelper dbHelper = new DbHelper.Oracle.DbHelper();

			//DbHelper.ConnectionString = ConfigurationManager.ConnectionStrings["MainDB"].ToString();
			DbHelper dbhelper = new DbHelper();
			var conn = dbhelper.Connection;
		}
	}
}
