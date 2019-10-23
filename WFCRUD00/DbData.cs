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
        protected static SQLiteConnection connection = new SQLiteConnection(@"Data Source=people.db;Version=3;");

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
                    return insert;
                }
            }
            connection.Close();

            return insert;
        }
    }
}
