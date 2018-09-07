using NoteWebApp.Models;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NoteWebApp.Areas.Lab.Controllers
{
	public class SqlInjectionController : Controller
	{
		// GET: Lab/SqlInjection
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult UnsecuredLogin(string id, string pass)
		{
			StringBuilder sb = new StringBuilder();

			//sb.AppendLine($"id: {id} / pass: {pass}");

			string sql = $"SELECT * FROM LAB_USER WHERE ID = '{id}' AND PASS = '{pass}'";

			// SQL Injection string: user0' OR '1' = '1

			sb.AppendLine($"SQL: {sql}");

			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					sb.AppendLine("로그인 성공");
				} 
				else
				{
					sb.AppendLine("로그인 실패");
				}

				reader.Close();

			};

			ViewBag.Message = sb.ToString().Replace(Environment.NewLine, "<br />");

			return View();
		}

		public ActionResult SecuredLogin(string id, string pass)
		{
			StringBuilder sb = new StringBuilder();

			//sb.AppendLine($"id: {id} / pass: {pass}");

			string sql = $"SELECT * FROM LAB_USER WHERE ID = :id AND PASS = :pass";

			// SQL Injection string: user0' OR '1' = '1

			sb.AppendLine($"SQL: {sql}");

			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleParameter paramId = cmd.Parameters.Add("id", OracleDbType.Varchar2);
				//param.Direction = ParameterDirection.Input;
				paramId.Value = id;

				OracleParameter paramPass = cmd.Parameters.Add("pass", OracleDbType.Varchar2);
				//param.Direction = ParameterDirection.Input;
				paramPass.Value = pass;

				OracleDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					sb.AppendLine("로그인 성공");
				}
				else
				{
					sb.AppendLine("로그인 실패");
				}

				reader.Close();

			};

			ViewBag.Message = sb.ToString().Replace(Environment.NewLine, "<br />");

			return View();
		}

	}
}