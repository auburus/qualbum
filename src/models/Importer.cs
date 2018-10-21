using Mono.Data.Sqlite;
using System;
using System.IO;
using System.Data;


namespace Qualbum
{

    public class Importer
    {
        private Library library;

        public Importer(Library library)
        {
            this.library = library;
            this.createDeletedTable();
        }

        public void Delete(FileInfo photoFile)
        {
            long id;

            using (SqliteConnection conn =
                    new SqliteConnection(this.library.ConnectionString))
            {
                conn.Open();

                SqliteCommand command = new SqliteCommand(conn);
                command.CommandText = 
                    @"INSERT INTO deleted_photos (path) VALUES
                    ('" + photoFile.FullName + "');";


                command.ExecuteScalar();

                command.CommandText = @"select last_insert_rowid();";
                id = (long)command.ExecuteScalar();
            }

            photoFile.MoveTo(
                Path.Combine(library.DeletedFolder.FullName, id.ToString())
            );
        }

        public FileInfo RestoreLast()
        {
            FileInfo photoFile = null;

            using (SqliteConnection conn =
                    new SqliteConnection(this.library.ConnectionString))
            {
                conn.Open();

                SqliteCommand command = new SqliteCommand(conn);
                command.CommandText = 
                    @"SELECT id, path FROM deleted_photos ORDER BY id DESC LIMIT 1;";


                SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read()) {
                    long id = (long)reader[0];
                    String path = (string)reader[1];

                    reader.Close();

                    photoFile = new FileInfo(
                        Path.Combine(library.DeletedFolder.FullName, id.ToString())
                    );

                    if (photoFile.Exists) {
                        // There are that many things that can go wrong in this line...
                        // i.e. folder no longer exists...
                        photoFile.MoveTo(path); 
                    }

                    command.CommandText =
                        @"DELETE FROM deleted_photos WHERE id=" + id.ToString() + ";";

                    command.ExecuteScalar();
                }
            }

            return photoFile;
        }

        /// Removes all deleted photos and records in the deleted table
        public void RemoveAllDeleted()
        {
            foreach(FileInfo f in library.DeletedFolder.GetFiles()) {
                f.Delete();
            }

            using (SqliteConnection conn =
                    new SqliteConnection(this.library.ConnectionString))
            {
                conn.Open();
                SqliteCommand command = new SqliteCommand(conn);
                command.CommandText = @"DROP TABLE deleted_photos;";
                command.ExecuteNonQuery();
            }

            this.createDeletedTable();
        }

        /// <sumamry>
        /// Creates the deleted table if it doesn't exist
        /// </sumamry>
        private void createDeletedTable()
        {
            using (SqliteConnection conn =
                    new SqliteConnection(this.library.ConnectionString))
            {
                conn.Open();
                SqliteCommand command = new SqliteCommand(conn);
                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS deleted_photos (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        path TEXT NOT NULL
                    );";

                command.ExecuteNonQuery();
            }
        }
    }
}
