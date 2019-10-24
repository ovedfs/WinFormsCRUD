//using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFCRUD00
{
    public class DbData
    {
        protected static SQLiteConnection connection = new SQLiteConnection(@"Data Source=C:\Users\OvedFS\Desktop\people.db;Version=3; FailIfMissing=True;");

        /*public DbData()
        {
            try
            {
                connection = new SQLiteConnection(@"Data Source=people.db;Version=3;");
                Debug.WriteLine("Connection estableshied...");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            
        }*/

        public static DataTable GetData()
        {
            connection.Open();
            
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM people", connection);
            
            DataSet dataSet = new DataSet();
            dataSet.Reset();

            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataSet);

            dataTable = dataSet.Tables[0];

            connection.Close();

            return dataTable;
        }

        public static int Insert(Person person)
        {
            connection.Open();
            int insert = -1;

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandText = "INSERT INTO people(firstName, lastName, email, phone) VALUES (@firstName, @lastName, @email, @phone)";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@firstName", person.FirstName);
                cmd.Parameters.AddWithValue("@lastName", person.LastName);
                cmd.Parameters.AddWithValue("@email", person.Email);
                cmd.Parameters.AddWithValue("@phone", person.Phone);
                try
                {
                    insert = cmd.ExecuteNonQuery();
                }
                catch (SQLiteException)
                {
                    connection.Close();
                    return insert;
                }
            }
            connection.Close();

            return insert;
        }

        public static int Update(Person person, int id)
        {
            /*connection.Open();
            int update = -1;

            using (SQLiteCommand cmd = new SQLiteCommand(connection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE people SET firstName = '@firstName', lastName = '@lastName', email = '@email', phone = '@phone' WHERE id = @id";
                //cmd.Prepare();
                cmd.Parameters.AddWithValue("@firstName", person.FirstName);
                cmd.Parameters.AddWithValue("@lastName", person.LastName);
                cmd.Parameters.AddWithValue("@email", person.Email);
                cmd.Parameters.AddWithValue("@phone", person.Phone);
                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    update = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    connection.Close();
                    return -1;
                }
            }
            connection.Close();

            return update;*/

            int update = -1;
            using (SQLiteConnection connection = new SQLiteConnection(@"Data Source=C:\Users\OvedFS\Desktop\people.db;Version=3; FailIfMissing=True;"))
            {
                string query = $"UPDATE people SET firstName = '{person.FirstName}', lastName = '{person.LastName}', email = '{person.Email}', phone = '{person.Phone}' WHERE id = {id}";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    /*cmd.Prepare();
                    cmd.Parameters.AddWithValue("@firstName", person.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", person.LastName);
                    cmd.Parameters.AddWithValue("@email", person.Email);
                    cmd.Parameters.AddWithValue("@phone", person.Phone);
                    cmd.Parameters.AddWithValue("@id", id);*/

                    try
                    {
                        connection.Open();
                        update = cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (SQLiteException e)
                    {
                        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                }
                //SQLiteCommand cmd = new SQLiteCommand(query, connection);
                
                //connection.Close();
            }

            return update;
        }

        public static Person Find(int id)
        {
            using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM people WHERE id = @id LIMIT 1", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Person person = new Person(reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString());
                    person.Id = Int32.Parse(reader[0].ToString());
                    connection.Close();
                    return person;
                }
            }

            connection.Close();
            return new Person();
        }
    }
}
