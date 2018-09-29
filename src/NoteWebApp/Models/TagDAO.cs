using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
	public class TagDAO
	{
		/// <summary>
		/// 태그 리스트 불러오기
		/// </summary>
		/// <returns>List<Tag></returns>
		public static List<TagVO> GetTagList()
		{
			List<TagVO> TagList = new List<TagVO>();

			OracleConnection conn = DbHelper.NewConnection();

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
				TagVO tag = new TagVO();
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
			using (OracleConnection conn = DbHelper.NewConnection())
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
					int newTagId = GetNewTagId();

					String sql = $"INSERT INTO TAG (TAG_ID, TAG_NAME ) VALUES (:TagId, :NAME)";


					OracleCommand cmd = new OracleCommand
					{
						Connection = conn,
						CommandText = sql
					};


					//error
					OracleParameter paramTagId = cmd.Parameters.Add("TagId", OracleDbType.Int32);
					paramTagId.Value = newTagId;

					OracleParameter paramName = cmd.Parameters.Add("Name", OracleDbType.Varchar2);
					paramName.Value = name;

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
		public static List<TagVO> Search(String keyword)
		{
			List<TagVO> TagList = new List<TagVO>();

			OracleConnection conn = DbHelper.NewConnection();

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
				TagVO tag = new TagVO();
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

			using (OracleConnection conn = DbHelper.NewConnection())
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
			TagVO newTag = new TagVO();

			using (OracleConnection conn = DbHelper.NewConnection())
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

					String sql = $"INSERT INTO TAG (TAG_ID, TAG_NAME) VALUES (:TAGID, :NAME)";

					OracleCommand cmd = new OracleCommand
					{
						Connection = conn,
						CommandText = sql
					};

					OracleParameter TagId = cmd.Parameters.Add("TAGID", OracleDbType.Int32);
					TagId.Value = GetNewTagId();

					OracleParameter Name = cmd.Parameters.Add("Name", OracleDbType.Varchar2);
					Name.Value = tagName;

					cmd.ExecuteNonQuery();

					newTag.Tag_Id = int.Parse(TagId.ToString());

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
		public static List<TagVO> GetTagListByNote(int noteId)
		{
			List<TagVO> tagList = new List<TagVO>();

			using (OracleConnection conn = DbHelper.NewConnection())
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
					TagVO tag = new TagVO();
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

			using (OracleConnection conn = DbHelper.NewConnection())
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
				if (reader.Read()) 
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
		public static List<NoteVO> GetNoteListByTagId(int tagId)
		{
			List<NoteVO> noteList = new List<NoteVO>();

			using (OracleConnection conn = DbHelper.NewConnection())
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
				while (reader.Read()) 
				{
					NoteVO note = new NoteVO();
					note = NoteDAO.GetNotebyId(int.Parse(reader["note_id"].ToString()));
					noteList.Add(note);
				}

				reader.Close();


				return noteList;
			}
		}

		public static void DeleteTag(int tagId)
		{
			using (OracleConnection conn = DbHelper.NewConnection())
			{
				conn.Open();

				String MapSql = $"DELETE FROM NOTE_TAG_MAP WHERE TAG_ID = :tagId";

				OracleCommand mapCmd = new OracleCommand
				{
					Connection = conn,
					CommandText = MapSql
				};

				OracleParameter paramMapId = mapCmd.Parameters.Add("tagId", OracleDbType.Varchar2);
				paramMapId.Value = tagId;

				mapCmd.ExecuteNonQuery();



				String tagSql = $"DELETE FROM TAG WHERE TAG_ID = :tagId"; ;

				OracleCommand tagCmd = new OracleCommand
				{
					Connection = conn,
					CommandText = tagSql
				};

				OracleParameter paramTagId = tagCmd.Parameters.Add("tagId", OracleDbType.Varchar2);
				paramTagId.Value = tagId;

				tagCmd.ExecuteNonQuery();

			}
		}

		/// <summary>
		/// 태그 이름 수정 함수
		/// (1) 중복 이름 검사
		/// (2) 매핑 테이블 검사 
		/// (3) (2)에 따라 db수정 
		/// </summary>
		public static string ModifyTagName(int tagId, string newTagName)
		{
			using (OracleConnection conn = DbHelper.NewConnection())
			{
				conn.Open();
				//빈칸 or 아무변화없을 경우
				

				//중복검사
				String checkSql = "select tag_name from tag where tag_name like :CheckName";

				OracleCommand checkCmd = new OracleCommand
				{
					Connection = conn,
					CommandText = checkSql
				};

				OracleParameter CheckName = checkCmd.Parameters.Add("CheckName", OracleDbType.Varchar2);
				CheckName.Value = newTagName;

				OracleDataReader reader = checkCmd.ExecuteReader();
				if (reader.HasRows) //중복 태그가 있는 경우
				{
					conn.Close();
					reader.Close();

					return newTagName + "이(가)라는 태그가 이미 있습니다.";

				}
				else // 중복 태그가 없는 경우
				{
					int newTagId = GetNewTagId();

					String sql = $"UPDATE TAG SET TAG_NAME = :NEWTAGNAME WHERE TAG_ID = :TAGID" ;


					OracleCommand cmd = new OracleCommand
					{
						Connection = conn,
						CommandText = sql
					};

					OracleParameter paramName = cmd.Parameters.Add("NEWTAGNAME", OracleDbType.Varchar2);
					paramName.Value = newTagName;

					OracleParameter paramTagId = cmd.Parameters.Add("TAGID", OracleDbType.Int32);
					paramTagId.Value = tagId;


					cmd.ExecuteNonQuery();

					conn.Close();
					reader.Close();

					return newTagName + " 로 수정했습니다.";
				}
			};
		}

		
	}
}