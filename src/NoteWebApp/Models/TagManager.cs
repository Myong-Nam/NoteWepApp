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

				}
				else // 중복 태그가 없는 경우
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

					return name + " 태그를 만들었습니다.";

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

		public static void AddTagToNote(string tagName, int noteId)
		{
			Tag newTag = new Tag();

			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				String checkSql = "select * from tag where tag_name like :CheckName";

				OracleCommand checkCmd = new OracleCommand
				{
					Connection = conn,
					CommandText = checkSql
				};

				OracleParameter CheckName = checkCmd.Parameters.Add("CheckName", OracleDbType.Varchar2);
				CheckName.Value = tagName;

				OracleDataReader reader = checkCmd.ExecuteReader();
				if (reader.HasRows) //중복 태그가 있는 경우
				{
					newTag.Tag_Id = int.Parse(reader["TAG_ID"].ToString());
					reader.Close();
				}
				else // 중복 태그가 없는 경우
				{
					int NewTagId = GetNewTagId();

					String sql = $"INSERT INTO TAG (TAG_ID, TAG_NAME) VALUES ({NewTagId}, :NAME)";

					OracleCommand cmd = new OracleCommand
					{
						Connection = conn,
						CommandText = sql
					};

					OracleParameter Name = cmd.Parameters.Add("Name", OracleDbType.Varchar2);
					Name.Value = tagName;

					cmd.ExecuteNonQuery();

					newTag.Tag_Id = NewTagId;

				}

				//note_tag_map 테이블에 관계 추가

				String mapSql = $"INSERT INTO NOTE_TAG_MAP (NOTE_ID, TAG_ID ) VALUES ({noteId},{newTag.Tag_Id})";

				OracleCommand mapCmd = new OracleCommand
				{
					Connection = conn,
					CommandText = mapSql
				};

				mapCmd.ExecuteNonQuery();

				conn.Close();

			}
		}
		/// <summary>
		/// 노트아이디로 태그리스트 찾음
		/// </summary>
		/// <param name="noteId"></param>
		/// <returns></returns>
		public static List<Tag> GetTagListByNote(int noteId)
		{
			List<Tag> tagList = new List<Tag>();

			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				//중복검사
				String sql = $"select tag_id from note_tag_map where note_id = :noteId";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleParameter paramnoteId = cmd.Parameters.Add("noteId", OracleDbType.Varchar2);
				paramnoteId.Value = noteId;

				OracleDataReader reader = cmd.ExecuteReader();
				while (reader.Read()) //
				{
					Tag tag = new Tag();
					tag.Tag_Id = int.Parse(reader["tag_id"].ToString());
					tag.Tag_Name = getTagNameByTagId(tag.Tag_Id);
					tagList.Add(tag);
				}

				return tagList;
			}

			
		}
		/// <summary>
		/// 태그아이디로 태그이름 찾음
		/// </summary>
		/// <param name="tagId"></param>
		/// <returns></returns>
		private static string getTagNameByTagId(int tagId)
		{
			string tagname = "";

			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				//중복검사
				String sql = $"select tag_name from tag where tag_id = :TagId";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleParameter paramTagId = cmd.Parameters.Add("TagId", OracleDbType.Varchar2);
				paramTagId.Value = tagId;

				OracleDataReader reader = cmd.ExecuteReader();
				if (reader.Read()) //
				{
					tagname = reader["tag_name"].ToString();
				}

				reader.Close();

				return tagname;
			}
		}

		/// <summary>
		/// 태그아이디로 노트리스트 불러옴
		/// </summary>
		/// <param name="tagId"></param>
		/// <returns></returns>
		public static List<Note> GetNoteListByTagId(int tagId)
		{
			List<Note> noteList = new List<Note>();

			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				//중복검사
				String sql = $"select note_id from note_tag_map where tag_id = :TagId";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleParameter paramTagId = cmd.Parameters.Add("TagId", OracleDbType.Varchar2);
				paramTagId.Value = tagId;

				OracleDataReader reader = cmd.ExecuteReader();
				while (reader.Read()) //
				{
					Note note = new Note();
					note = NoteManager.GetNotebyId(int.Parse(reader["note_id"].ToString()));
					noteList.Add(note);
				}

				reader.Close();


				return noteList;
			}
		}
	}
}