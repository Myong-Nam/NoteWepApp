using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Controllers
{
	/// <summary>
	/// 모든 Controller는 BaseController를 상속받아야 한다
	/// </summary>
	public class BaseController : Controller
	{
		DateTime _actionTime_Begin;
		DateTime _actionTime_End;
		DateTime _resultTime_Begin;
		DateTime _resultTime_End;

		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.OnAuthorization(filterContext);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("[{0}] Beginning of Action.", filterContext.HttpContext.Request.HttpMethod);

			if (string.IsNullOrEmpty(filterContext.HttpContext.Request.Url.Query) == false)
			{
				sb.AppendLine();
				sb.AppendFormat("Url Query: {0}", filterContext.HttpContext.Request.Url.Query);
			}

			if (filterContext.HttpContext.Request.UrlReferrer != null)
			{
				sb.AppendFormat(" [From] {0}", filterContext.HttpContext.Request.UrlReferrer.OriginalString);
			}

			var logger = NLog.LogManager.GetCurrentClassLogger();
			//logger.Info(sb.ToString());

			// Time recording
			_actionTime_Begin = DateTime.Now;
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			_actionTime_End = DateTime.Now;

			var logger = NLog.LogManager.GetCurrentClassLogger();
			//logger.Info($"[{filterContext.HttpContext.Request.HttpMethod}] End of Action. {(_actionTime_End - _actionTime_Begin).TotalSeconds.ToString("f")} seconds elapsed.");

			base.OnActionExecuted(filterContext);
		}

		protected override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			base.OnResultExecuting(filterContext);

			var logger = NLog.LogManager.GetCurrentClassLogger();
			//logger.Info("[View] Beginning of rendering.");

			// Time recording
			_resultTime_Begin = DateTime.Now;
		}

		protected override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			_resultTime_End = DateTime.Now;

			var logger = NLog.LogManager.GetCurrentClassLogger();
			//logger.Info($"[View] End of rendering. {(_actionTime_End - _actionTime_Begin).TotalSeconds.ToString("f")} seconds elapsed.");

			base.OnResultExecuted(filterContext);
		}

		protected override void OnException(ExceptionContext filterContext)
		{
			base.OnException(filterContext);

			var logger = NLog.LogManager.GetCurrentClassLogger();
			//logger.Info(filterContext.Exception.ToString());
		}
	}
}