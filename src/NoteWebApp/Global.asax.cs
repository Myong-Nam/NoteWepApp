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
			var logger = NLog.LogManager.GetCurrentClassLogger();

			logger.Info(System.Environment.NewLine);
			logger.Info(System.Environment.NewLine);
			logger.Info(System.Environment.NewLine);
			logger.Info(System.Environment.NewLine);
			logger.Info("================================ Application Start ======================================");

			AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

			//
			// Database setting
			//
			DbHelper.ConnectionString = ConfigurationManager.ConnectionStrings["MainDB"].ToString();


		}
	}
}
