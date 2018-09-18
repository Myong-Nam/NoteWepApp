using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
	public class NoteBookDAO
	{

		//노트북리스트 불러오기 : /index
		/*
		 목적 : 노트북 리스트 보여주기
		 준비물 : 노트북리스트, db커넥션
		 (1) 빈 노트북 리스트를 만든다.
		 (2) db커넥션을 생성하여 notebook을 모두 불러옴
		 (3) 빈 노트북리스트에 (2)에서 받아온 notebook을 넣음.
			 */
		public static List<NoteBookVO> GetNoteBookList()
		{
			List<NoteBookVO> noteBooks = new List<NoteBookVO>();

			OracleConnection conn = DbHelper.NewConnection();

			conn.Open();

			String sql = "select * from NoteBook";

			OracleCommand cmd = new OracleCommand
			{
				Connection = conn,
				CommandText = sql
			};

			OracleDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				NoteBookVO noteBook = new NoteBookVO();
				noteBook.NoteBookId = int.Parse(reader["NOTEBOOKID"].ToString());
				noteBook.Name = reader["NAME"].ToString();

				noteBooks.Add(noteBook);
			}

			reader.Close();
			conn.Close();

			return noteBooks;
		}

		// 새 노트북 생성 : /create
		/*
		 목적 : 새로운 노트북을 만듬
		 준비물 : 새로운 노트북아이디, db커넥션, 이름
		 (1) GetNewNoteBookId()를 통해 새로운 노트북 id를 생성
		 (2) db커넥션을 생성하여 해당 노트북아이디에 이름을 넣고 새로운 노트북 생성
			 */
		public static int Create(String name)
		{
			int NewNoteBookId = GetNewNoteBookId();

			using (OracleConnection conn = DbHelper.NewConnection())
			{
				conn.Open();

				String sql = $"Insert into notebook (notebookid, name, createddate, isdeleted) values ( {NewNoteBookId}, :Name, sysdate, {0})";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleParameter paramName = cmd.Parameters.Add("Name", OracleDbType.Varchar2);
				paramName.Value = name;

				cmd.ExecuteNonQuery();

			};

			return NewNoteBookId;
		}

		// 새 노트 생성 시, 새 노트북 아이디 구하기
		/*
		 목적 : 새로운 노트북 아이디 값을 구함
		 준비물 : 아이디를 넣을 빈 int값, db커넥션
		 (1) db에서 notebook 시퀀스의 nextvalue값을 얻어옴.
			 */
		private static int GetNewNoteBookId()
		{
			int NewNoteBookId = new int();

			using (OracleConnection conn = DbHelper.NewConnection())
			{
				conn.Open();

				String sql = "SELECT NOTEBOOK_SEQ.NEXTVAL FROM DUAL";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					NewNoteBookId = int.Parse(reader[0].ToString());
				}

				reader.Close();

			};

			return NewNoteBookId;

		}

		// 아이디로 노트북 불러오기
		/*
		 목적 : 노트북을 불러옴
		 준비물 : 빈 노트북, 노트북아이디, db커넥션
		 (1) 빈 노트북 생성
		 (2) db커넥션을 생성하여 해당 노트북아이디를 가진 노트북 부름
		 (3) 빈 노트북의 해당 노트북 정보 넣음
			 */
		public static NoteBookVO GetNoteBookbyId(int noteBookId)
		{
			NoteBookVO noteBook = new NoteBookVO();

			OracleConnection conn = DbHelper.NewConnection();

			conn.Open();

			String sql = $"select * from NoteBook where noteBookId = {noteBookId}";

			OracleCommand cmd = new OracleCommand
			{
				Connection = conn,
				CommandText = sql
			};

			OracleDataReader reader = cmd.ExecuteReader();
			if (reader.Read())
			{
				noteBook.NoteBookId = int.Parse(reader["NOTEBOOKID"].ToString());
				noteBook.Name = reader["NAME"].ToString();
				noteBook.IsDefault = int.Parse(reader["ISDEFAULT"].ToString());
			}
			reader.Close();
			conn.Close();


			return noteBook;
		}



		//노트북 완전 삭제 : /detail
		/*
		 목적 : 노트북 삭제
		 준비물 : 노트북아이디, db커넥션
		 (1) 해당 노트북아이디를 가진 노트를 선택하여, 노트북 아이디를 기본노트북의 아이디로 바꿈
		 (2) 해당 노트북을 삭제
			 */
		public static void Delete(int noteBookId)
		{
			int defaultNoteBook = DefaultNoteBook();

			using (OracleConnection conn = DbHelper.NewConnection())
			{
				conn.Open();

				String NoteSql = $"UPDATE note SET notebookid = {defaultNoteBook}, isdeleted = {1} WHERE notebookid = {noteBookId}";

				OracleCommand NoteCmd = new OracleCommand
				{
					Connection = conn,
					CommandText = NoteSql
				};

				NoteCmd.ExecuteNonQuery();

				String sql = $"Delete FROM notebook WHERE noteBookId = {noteBookId}";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				cmd.ExecuteNonQuery();

			}

		}

		// 노트북 수정 : /Info
		/*
		 목적 : 노트북 이름 수정
		 준비물 : 노트북아이디, 이름
		 (1) 해당 아이디를 가진 노트북을 선택하여 이름을 바꿈 */

		public static void Update(int noteBookId, string name, int isdefault)
		{
			using (OracleConnection conn = DbHelper.NewConnection())
			{
				conn.Open();

				string nameSql = $"UPDATE notebook SET name = :name WHERE notebookid = {noteBookId}";
				string sql = "";

				if (isdefault == 1)
				{
					sql = $"UPDATE NOTEBOOK SET ISDEFAULT = case when notebookid != {noteBookId} then 0 when notebookid = {noteBookId} then 1 end";

					OracleCommand defaultCommand = new OracleCommand
					{
						Connection = conn,
						CommandText = sql
					};

					OracleParameter paramName = defaultCommand.Parameters.Add("name", OracleDbType.Varchar2);
					paramName.Value = name;

					defaultCommand.ExecuteNonQuery();

				}

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = nameSql
				};

				cmd.ExecuteNonQuery();
			};
		}


		//기본 노트북 id 찾는 함수
		public static int DefaultNoteBook()
		{
			int defaultNoteBook = 0;
			using (OracleConnection conn = DbHelper.NewConnection())
			{
				conn.Open();

				String sql = $"SELECT notebookid FROM notebook WHERE isdefault = {1}";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					defaultNoteBook = int.Parse(reader["NOTEBOOKID"].ToString());
				}
				reader.Close();

				return defaultNoteBook;

			}
		}


		//해당 노트북이 기본 노트북인지 아닌지 판별함
		private static Boolean IsDefaultNoteBook(int noteBookId)
		{
			int isDefault = 0;
			using (OracleConnection conn = DbHelper.NewConnection())
			{
				conn.Open();

				String sql = $"SELECT isdefault FROM notebook WHERE notebookid = {noteBookId}";

				OracleCommand cmd = new OracleCommand
				{
					Connection = conn,
					CommandText = sql
				};

				OracleDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					isDefault = int.Parse(reader["ISDEFAULT"].ToString());
				}
				reader.Close();

				if (isDefault == 1)
				{
					return true;
				}
				else
				{
					return false;
				}

			};



		}

		//해당 노트북 노트 개수구하기
		public static int CountInBook(int bookId)
		{
			List<NoteVO> List = NoteDAO.GetNoteList(OrderColumn.Notedate, OrderType.Desc, bookId);
			int count = List.Count;

			return count;
		}

	}
}