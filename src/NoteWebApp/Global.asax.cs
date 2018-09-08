using DbHelper;
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
			Database database = new Database();
			database.ConnectionString = ConfigurationManager.ConnectionStrings["MainDB"].ToString();
			App.Database = database;


		}
	}
}
