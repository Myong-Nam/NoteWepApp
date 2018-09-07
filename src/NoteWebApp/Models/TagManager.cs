using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
	public class TagManager
	{
		/// <summary>
		/// 태그 리스트 불러오기
		/// </summary>
		/// <returns>List<Tag></returns>
		public static List<Tag> GetTagList()
		{
			List<Tag> TagList = new List<Tag>();

			OracleConnection conn = new OracleConnection(DataBase.ConnectionString);

			conn.Open();

			String sql = "select * from Tag";

			OracleCommand cmd = new OracleCommand
			{
				Connection = conn,
				CommandText = sql
			};

			OracleDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				Tag tag = new Tag();
				tag.Tag_Id = int.Parse(reader["TAG_ID"].ToString());
				tag.Tag_Name = reader["TAG_NAME"].ToString();

				TagList.Add(tag);
			}


			reader.Close();
			conn.Close();

			return TagList;
		}
		/// <summary>
		/// 태그 새로 생성
		/// </summary>
		/// <param name="name">태그 이름</param>
		public static string Create(String name)
		{
			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				//중복검사
				String checkSql = "select tag_name from tag where tag_name like :CheckName";

				OracleCommand checkCmd = new OracleCommand
				{
					Connection = conn,
					CommandText = checkSql
				};

				OracleParameter CheckName = checkCmd.Parameters.Add("CheckName", OracleDbType.Varchar2);
				CheckName.Value = name;

				OracleDataReader reader = checkCmd.ExecuteReader();
				if (reader.HasRows) //중복 태그가 있는 경우
				{
					conn.Close();
					reader.Close();

					return name + "이(가)라는 태그가 이미 있습니다.";

				} else // 중복 태그가 없는 경우
				{
					int NewTagId = GetNewTagId();

					String sql = $"INSERT INTO TAG (TAG_ID, TAG_NAME ) VALUES ({NewTagId}, :NAME)";


					OracleCommand cmd = new OracleCommand
					{
						Connection = conn,
						CommandText = sql
					};

					OracleParameter Name = cmd.Parameters.Add("Name", OracleDbType.Varchar2);
					Name.Value = name;

					cmd.ExecuteNonQuery();

					conn.Close();
					reader.Close();

					return name + "태그를 만들었습니다.";

				}


				

			};

		}

		/// <summary>
		/// 태그 검색
		/// </summary>
		/// <param name="keyword">사용자가 입력하는 검색어</param>
		/// <returns></returns>
		public static List<Tag> Search(String keyword)
		{
			List<Tag> TagList = new List<Tag>();

			OracleConnection conn = new OracleConnection(DataBase.ConnectionString);

			conn.Open();

			String sql = "select * from Tag";

			OracleCommand cmd = new OracleCommand
			{
				Connection = conn,
				CommandText = sql
			};

			OracleDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				Tag tag = new Tag();
				tag.Tag_Id = int.Parse(reader["TAG_ID"].ToString());
				tag.Tag_Name = reader["TAG_NAME"].ToString();

				TagList.Add(tag);
			}


			reader.Close();
			conn.Close();

			return TagList;

		}


		private static int GetNewTagId()
		{
			int NewTagId = new int();

			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				String sql = "SELECT TAG_SEQ.NEXTVAL FROM DUAL";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					NewTagId = int.Parse(reader[0].ToString());
				}

				reader.Close();

			};

			return NewTagId;

		}
	}
}