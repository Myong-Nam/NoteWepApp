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

        // 노트북 아이디로 노트 불러오기 : /detail
        public static NoteBook GetNotebyId(int noteBookId)
        {
            

            return null;
        }

        // 새 노트북 생성 : /create
        public static void Create(String Name)
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

  

        //노트북 완전 삭제 : /detail
        public static void Delete(int noteBookId)
        {
            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

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

                String sql = $"UPDATE notebook SET name = '{name}', WHERE notebookid = {noteBookId}";

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