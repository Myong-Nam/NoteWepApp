using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoteWebApp
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


			//노트 삭제
			routes.MapRoute(
				name: "Delete Note Route",
				url: "Note/Delete",
				defaults: new { controller = "Note", action = "Delete" }
			);

			//노트 수정
			routes.MapRoute(
				name: "Update Note Route",
				url: "Note/Update",
				defaults: new { controller = "Note", action = "Update" }
			);

			//노트 추가
			routes.MapRoute(
				name: "Create Note Route",
				url: "Note/Create",
				defaults: new { controller = "Note", action = "Create" }
			);

			//노트 보기
			routes.MapRoute(
				name: "Note Route",
				url: "Note/{id}",
				defaults: new { controller = "Note", action = "Detail", id = UrlParameter.Optional }
			);

			//기본 
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Note", action = "Detail", id = UrlParameter.Optional }
			);
		}


	}
}
