using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
    public class NoteBookManager
    {

        //노트북리스트 불러오기 : /index
        /*
         목적 : 노트북 리스트 보여주기
         준비물 : 노트북리스트, db커넥션
         (1) 빈 노트북 리스트를 만든다.
         (2) db커넥션을 생성하여 notebook을 모두 불러옴
         (3) 빈 노트북리스트에 (2)에서 받아온 notebook을 넣음.
             */
        public static List<NoteBook> GetNoteBookList()
        {
            List<NoteBook> noteBooks = new List<NoteBook>();
            
            OracleConnection conn = new OracleConnection(DataBase.ConnectionString);

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
                NoteBook noteBook = new NoteBook();
                noteBook.NoteBookId = int.Parse(reader["NOTEBOOKID"].ToString());
                noteBook.Name = reader["NAME"].ToString();


                noteBooks.Add(noteBook);
            }
            reader.Close();
            conn.Close();

            return noteBooks;
        }

        // 노트북 아이디로 노트리스트 불러오기 : /List
        /*
         목적 : 노트북 내에 있는 노트들을 보여줌
         준비물 : 노트북아이디, db커넥션
         (1) 빈 노트 리스트를 만든다.
         (2) db커넥션을 생성하여 해당 노트북아이디를 가진 note를 모두 불러옴
         (3) 빈 노트리스트에 (2)에서 받아온 note를 넣음.
             */
        public static List<Note> NotesInNoteBook(int id)
        {
            List<Note> noteList = new List<Note>();

            OracleConnection conn = new OracleConnection(DataBase.ConnectionString);

            conn.Open();

            String sql = $"select * from Note where isdeleted = {0} and notebookid = {id}";

            OracleCommand cmd = new OracleCommand
            {
                Connection = conn,
                CommandText = sql
            };

            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Note note = new Note
                {
                    NoteId = int.Parse(reader["NOTEID"].ToString()),
                    Title = reader["TITLE"].ToString(),
                };

                noteList.Add(note);
            }
            reader.Close();
            conn.Close();

            return noteList;
        }

        // 새 노트북 생성 : /create
        /*
         목적 : 새로운 노트북을 만듬
         준비물 : 새로운 노트북아이디, db커넥션, 이름
         (1) GetNewNoteBookId()를 통해 새로운 노트북 id를 생성
         (2) db커넥션을 생성하여 해당 노트북아이디에 이름을 넣고 새로운 노트북 생성
             */
        public static int Create(String Name)
        {
            int NewNoteBookId = GetNewNoteBookId();

            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

                String sql = $"Insert into notebook (notebookid, name, createddate, isdeleted) values ( {NewNoteBookId}, '{Name}', sysdate, {0})";

                OracleCommand cmd = new OracleCommand
                {
                    Connection = conn,
                    CommandText = sql
                };

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

            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
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
        public static NoteBook GetNoteBookbyId(int noteBookId)
        {
            NoteBook noteBook = new NoteBook();

            OracleConnection conn = new OracleConnection(DataBase.ConnectionString);

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
            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

                String NoteSql = $"UPDATE note SET notebookid = {1}, isdeleted = {1} WHERE notebookid = {noteBookId}";

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

        // 노트북 수정 : /detail
        /*
         목적 : 노트북 이름 수정
         준비물 : 노트북아이디, 이름
         (1) 해당 아이디를 가진 노트북을 선택하여 이름을 바꿈 */

        public static void Update(int noteBookId, string name)
        {
            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

                String sql = $"UPDATE notebook SET name = '{name}' WHERE notebookid = {noteBookId}";

                OracleCommand cmd = new OracleCommand
                {
                    Connection = conn,
                    CommandText = sql
                };

                cmd.ExecuteNonQuery();
            };
        }
  
    }
}