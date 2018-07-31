using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
    public class ControlController : Controller
    {
        // GET: Lab/Control
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult ListSortOrder(string NoteBookId, string OrderColumnName, string OrderType)
		{
			// default values
			string defaultNoteBookId = "";
			string defaultOrderColumnName = "title";
			string defaultOrderType = "asc";

			// final values
			string finalNoteBookId = (String.IsNullOrEmpty(NoteBookId)) ? defaultNoteBookId : NoteBookId;
			string finalOrderColumnName = (String.IsNullOrEmpty(OrderColumnName)) ? defaultOrderColumnName : OrderColumnName;
			string finalOrderType = (String.IsNullOrEmpty(OrderType)) ? defaultOrderType : OrderType;

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("리스트 내용");

			if (String.IsNullOrEmpty(finalNoteBookId))
			{
				sb.AppendLine($"모든 notebook에서 노트 목록을 가져옵니다.");
			}
			else
			{
				sb.AppendLine($"{NoteBookId}번 notebook의 노트 목록을 가져옵니다.");
			}

			sb.AppendLine($"{finalOrderColumnName} 컬럼을 기준으로 {finalOrderType} 정렬합니다.");

			ViewBag.List = sb.ToString();

			ViewBag.NoteNookId = finalNoteBookId;
			ViewBag.OrderColumnName = finalOrderColumnName;
			ViewBag.OrderType = finalOrderType;

			return View();
		}
    }
}