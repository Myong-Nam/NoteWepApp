using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Views.Note
{
	/// <summary>
	/// Upload의 요약 설명입니다.
	/// </summary>
	public class Upload : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			HttpPostedFile uploads = context.Request.Files["upload"];
			string CKEditorFuncNum = context.Request["CKEditorFuncNum"];
			string file = System.IO.Path.GetFileName(uploads.FileName);
			uploads.SaveAs(context.Server.MapPath(".") + "\\Image\\" + file);
			//provide direct URL here
			string url = "http://localhost:7223/Handler/Image/" + file;

			context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction( "+CKEditorFuncNum +  ", \"" + url + "\");</script>");
			context.Response.End();
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}