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
        public static int GetNewNoteBookId()
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
        public static void Delete(int noteBookId)
        {
            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

                String NoteSql = $"UPDATE note SET notebookid = {1} WHERE notebookid = {noteBookId}";

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