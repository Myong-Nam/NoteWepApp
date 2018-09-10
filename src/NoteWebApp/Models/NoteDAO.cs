
using Dapper;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NoteWebApp.Models
{
	public class NoteDAO
	{
		/// <summary>
		/// 목적 : 인덱스 페이지에서 노트리스트를 최근순으로 불러옴
		/// 준비물 : 노트리스트, db커넥션
		/// (1) 빈 노트리스트와 db커넥션을 생성.
		/// (2) isdeleted의 값이 0인(삭제되지 않은) 노트를 불러오는 쿼리 작성.
		/// (3) 불러온 노트를 노트리스트에 넣고 커넥션과 reader를 닫아준다.
		/// </summary>
		/// <param name="orderColumnName">"notedate" | "title"</param>
		/// <param name="orderType">"ASC" | "DESC"</param>
		/// <param name="noteBookId"></param>
		/// <returns></returns>
		public static List<NoteVO> GetNoteList(OrderColumn orderColumnName, OrderType orderType, int noteBookId)
		{
			List<NoteVO> noteList = new List<NoteVO>();

			OracleConnection conn = new OracleConnection(DataBase.ConnectionString);

			conn.Open();

			StringBuilder sbQuery = new StringBuilder();

			sbQuery.Append("\n SELECT  ");
			sbQuery.Append("\n     noteid,  ");
			sbQuery.Append("\n     title, ");
			sbQuery.Append("\n     contents, ");
			sbQuery.Append("\n     notedate, ");
			sbQuery.Append("\n     TO_CHAR(notedate,'YYMMDD')AS yymmdd ");
			sbQuery.Append("\n FROM ");
			sbQuery.Append("\n     note ");
			if (noteBookId == 0) //삭제 되지 않은 노트 전체
			{
				sbQuery.Append("\n WHERE isdeleted = 0 ");
			}
			else if (noteBookId == -1) //휴지통에 있는 노트 전체
			{
				sbQuery.Append("\n WHERE isdeleted = 1 ");

			}
			else
			{
				sbQuery.Append("\n WHERE isdeleted = 0 ");
				sbQuery.Append("\n AND notebookid = " + noteBookId);
			}

			sbQuery.Append($"\n order by {orderColumnName} {orderType}");

			String sql = sbQuery.ToString();

			OracleCommand cmd = new OracleCommand
			{
				Connection = conn,
				CommandText = sql
			};


			OracleDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				NoteVO note = new NoteVO
				{
					NoteId = int.Parse(reader["NOTEID"].ToString()),
					Title = reader["TITLE"].ToString(),
					Contents = reader["CONTENTS"] as String,
					NoteDate = reader["YYMMDD"].ToString(),
					FullDate = Convert.ToDateTime(reader["NOTEDATE"])
				};

				noteList.Add(note);

				DateTime today = DateTime.Now;
				DateTime theDay = note.FullDate;

				TimeSpan diff = today.Subtract(theDay);

				if (diff.Days < 21)
				{
					note.NoteDate = "2주 전";

					if (diff.Days < 14)
					{
						note.NoteDate = "1주 전";

						if (diff.Days < 7)
						{
							note.NoteDate = diff.Days.ToString() + "일 전";

							if (diff.Days < 1)
							{
								note.NoteDate = diff.Hours.ToString() + "시간 전";

								if (diff.Hours < 1)
								{
									note.NoteDate = diff.Minutes.ToString() + "분 전";

									if (diff.Minutes < 1)
									{
										note.NoteDate = diff.Seconds.ToString() + "초 전";
									}
								}

							}
						}
					}
				}

				else
				{
					note.NoteDate = note.FullDate.ToString();
					//note.NoteDate = note.FullDate.ToString("yy. M. d.");
				}


			}
			reader.Close();
			conn.Close();



			return noteList;
		}


		// 노트 아이디로 노트 불러오기 : /detail
		/*
		 목적 : 노트 아이디로 노트의 정보를 불러옴
		 준비물 : 노트, db커넥션
		 (1) 빈 노트와 db커넥션을 생성.
		 (2) 노트의 정보를 불러오는 쿼리 작성
		 (3) 불러온 정보를 빈 노트에 넣어준다.
			 */
		public static NoteVO GetNotebyId(int noteId)
		{
			NoteVO note = new NoteVO();

			String sql = "select * from Note where noteId = " + noteId.ToString();

			using (var conn = DbHelper.Connection)
			{
				note = conn.QuerySingle<NoteVO>(sql);
			}

			//OracleConnection conn = new OracleConnection(DataBase.ConnectionString);

			//conn.Open();

			//String sql = "select * from Note where noteId = " + noteId.ToString();

			//OracleCommand cmd = new OracleCommand
			//{
			//	Connection = conn,
			//	CommandText = sql
			//};

			//OracleDataReader reader = cmd.ExecuteReader();
			//while (reader.Read())
			//{
			//	NoteVO newNote = new NoteVO
			//	{
			//		NoteId = int.Parse(reader["NOTEID"].ToString()),
			//		Title = reader["TITLE"].ToString(),
			//		IsDeleted = int.Parse(reader["ISDELETED"].ToString()),
			//		Contents = reader["CONTENTS"] as String,
			//		NoteDate = reader["NOTEDATE"].ToString(),
			//		UpdatedDate = reader["UpdatedDate"].ToString(),
			//		NoteBookId = int.Parse(reader["NOTEBOOKID"].ToString())
			//	};

			//	note = newNote;
			//}
			//reader.Close();
			//conn.Close();


			return note;  
		}

		// 새 노트 작성 : /Create
		/*
		 목적 : 새로운 노트를 생성함
		 준비물 : 새노트 아이디, 제목, 본문, db커넥션
		 (1) GetNewNoteId()를 통해 새로은 노트 아이디를 받음
		 (2) 새노트 아이디, 제목, 본문, 작성일자를 db로 insert해줌.

			 */
		public static void Create(String Title, String Contents, string notebookid)
		{
			int NewNoteId = GetNewNoteId();
			int NewBookId = int.Parse(notebookid);
			

			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				String sql = $"Insert into note (noteid, title, contents, notedate, updateddate, notebookid) values ( {NewNoteId}, '{Title}', '{Contents}', sysdate, sysdate, {NewBookId})";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				cmd.ExecuteNonQuery();

			};

		}

		// 새 노트 생성 시, 새 노트 아이디 구하기
		/*
		 목적 : 새 노트의 아이디를 구함
		 준비물 : 빈 int변수, db커넥션
		 (1) 빈 int 값을 생성함
		 (2) db커넥션을 생성하여 노트북 시퀀스에 있는 다음 값(NEXVAL)을 받아옴.
		 (3) 빈 INT값에 (2)에서 얻어온 값을 넣음.
		*/
		public static int GetNewNoteId()
		{
			int NewNoteId = new int();

			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				String sql = "SELECT NOTE_SEQ.NEXTVAL FROM DUAL";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					NewNoteId = int.Parse(reader[0].ToString());
				}

				reader.Close();

			} ;
		
			return NewNoteId;
		}

		// 노트 휴지통으로 보내기 : /detail
		/*
		 목적 : 노트를 휴지통으로 보냄
		 준비물 : 노트 아이디. db커넥션
		 (1) db커넥션을 생성하
		 (2) 해당 노트 아이디를 이용해 삭제여부(isdeleted)을 true(1)로, 노트북을 기본노트북으로 변경.
		*/
		public static void preDelete(int noteId)
		{
			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				String sql = $"UPDATE note SET IsDeleted = {1}, notebookid = {1} WHERE noteid = {noteId}";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				cmd.ExecuteNonQuery();
			}
		}

		// 휴지통의 노트 완전 삭제 : /detail
		/*
		 목적 : 노트를 휴지통으로 보냄
		 준비물 : 노트 아이디. db커넥션
		 (1) db커넥션을 생성하
		 (2) 해당 노트 아이디를 이용해 삭제여부(isdeleted)을 true(1)로, 노트북을 기본노트북으로 변경.
		*/
		public static int Delete(int noteId)
		{
			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				String sql = $"Delete FROM note WHERE noteid = {noteId}";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				cmd.ExecuteNonQuery();
			}

			return noteId;
		}

		// 노트 수정 : /detail
		/*
		 목적 : 노트 디테일 화면에서 제목, 내용, 노트북 등을 수정하게함.
		 준비물 : 노트 아이디, 제목, 내용, 노트북아이디, db커넥션
		 (1) 노트북아이디는 드롭다운에서 string값으로 값을 받아오므로, 그 값을 int로 변환
		 (2) db 커넥션 생성, 제목과 내용, (1)의 노트북아이디 값을 update함.
		*/
		public static void Update(int noteId, string title, string contents, string noteBookId)
		{
			int id = int.Parse(noteBookId);

			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				String sql = $"UPDATE note SET title = '{title}', contents = '{contents}', notebookid = {id}, updateddate = sysdate WHERE noteid = {noteId}";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				cmd.ExecuteNonQuery();
			};
		}

		// 휴지통에 있는 노트 복원 : /detail
		 /*
		 목적 : 사용자가 휴지통으로 보낸 노트들을 복원함.
		 준비물 : 노트아이디, db커넥션
		 (1) 노트 아이디로 db에서 해당 노트를 찾아 삭제여부를 false로 바꿈.
		*/
		public static void RecoverNote(int noteId)
		{
			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				String sql = $"UPDATE note SET IsDeleted = {0} WHERE noteid = {noteId}";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				cmd.ExecuteNonQuery();
			}
		}

		public static void ToShortCut(int noteid)
		{
			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				String sql = $"UPDATE note SET ISSHORTCUT = {1} WHERE noteid = {noteid}";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				cmd.ExecuteNonQuery();
			}
		}

		//create 뷰에서 저장버튼 눌렀을때 호출되는 메소드. 자동저장되는 id와 같도록 할것.
		public static string NewestNote()
		{
			string NewestNote = "";

			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				String sql = "select max(noteid) from Note order by notedate desc";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					int temp = int.Parse(reader["MAX(NOTEID)"].ToString()) + 1;
					NewestNote = temp.ToString();

				}

				reader.Close();

			};

			return NewestNote;
		}




		public static void NotToShortCut(int noteid)
		{
			using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
			{
				conn.Open();

				String sql = $"UPDATE note SET ISSHORTCUT = {0} WHERE noteid = {noteid}";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				cmd.ExecuteNonQuery();
			}
		}
	}
}