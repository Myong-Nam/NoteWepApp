
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWebApp.Models
{
    public class NoteManager
    {

        //노트리스트 불러오기 : /index
        public static List<Note> GetNoteList()
        {
            List<Note> noteList = new List<Note>();

            OracleConnection conn = new OracleConnection(DataBase.ConnectionString);

            conn.Open();

            String sql = $"select * from Note where isdeleted = {0} ORDER BY notedate desc";

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
                    Contents = reader["CONTENTS"] as String
                };

                noteList.Add(note);
            }
            reader.Close();
            conn.Close();

            return noteList;
        }

        // 노트 아이디로 노트 불러오기 : /detail
        public static Note GetNotebyId(int noteId)
        {
            Note note = new Note();

            OracleConnection conn = new OracleConnection(DataBase.ConnectionString);

            conn.Open();

            String sql = "select * from Note where noteId = " + noteId.ToString();

            OracleCommand cmd = new OracleCommand
            {
                Connection = conn,
                CommandText = sql
            };

            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Note newNote = new Note
                {
                    NoteId = int.Parse(reader["NOTEID"].ToString()),
                    Title = reader["TITLE"].ToString(),
                    IsDeleted = int.Parse(reader["ISDELETED"].ToString()),
                    Contents = reader["CONTENTS"] as String,
                    NoteDate = reader["NOTEDATE"].ToString(),
                    NoteBookId = int.Parse(reader["NOTEBOOKID"].ToString())
                };

                note = newNote;
            }
            reader.Close();
            conn.Close();


            return note;  
        }

        // 새 노트 생성 : /create
        public static int Create(String Title, String Contents)
        {
            int NewNoteId = GetNewNoteId();

            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

                String sql = $"Insert into note (noteid, title, contents, notedate) values ( {NewNoteId}, '{Title}', '{Contents}', sysdate)";

                OracleCommand cmd = new OracleCommand
                {
                    Connection = conn,
                    CommandText = sql
                };

                cmd.ExecuteNonQuery();

            };

            return NewNoteId;
        }

        // 새 노트 생성 시, 새 노트 아이디 구하기
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
        public static void preDelete(int noteId)
        {
            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

                String sql = $"UPDATE note SET IsDeleted = {1} WHERE noteid = {noteId}";

                OracleCommand cmd = new OracleCommand
                {
                    Connection = conn,
                    CommandText = sql
                };

                cmd.ExecuteNonQuery();
            }
        }

        // 휴지통의 노트 완전 삭제 : /detail
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
        public static void Update(int noteId, string title, string contents, string noteBookId)
        {
            int id = int.Parse(noteBookId);

            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

                String sql = $"UPDATE note SET title = '{title}', contents = '{contents}', notebookid = {id} WHERE noteid = {noteId}";

                OracleCommand cmd = new OracleCommand
                {
                    Connection = conn,
                    CommandText = sql
                };

                cmd.ExecuteNonQuery();
            };
        }

        // 휴지통에 있는 노트리스트 불러오기 : /deleted
        public static List<Note> GetDeletedNoteList()
        {
            List<Note> noteList = new List<Note>();

            using (OracleConnection conn = new OracleConnection(DataBase.ConnectionString))
            {
                conn.Open();

                String sql = $"select * from Note where ISDELETED = {1} ORDER BY notedate desc";

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

            }

            return noteList;
        }

        // 휴지통에 있는 노트 복원 : /detail
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
    }
}