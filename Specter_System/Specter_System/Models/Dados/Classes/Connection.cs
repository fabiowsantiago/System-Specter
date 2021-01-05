using MySql.Data.MySqlClient;
using System;

namespace Specter_System.Models.Dados.Classes
{
    public class Connection
    {
        private MySqlConnection connect;

        protected Connection()
        {
            this.connect = new MySqlConnection();
            this.connect.ConnectionString = "SERVER=localhost; PORT=3306; USER ID=root; DATABASE=expertnutri; PASSWORD=fwss198321";
        }

        protected MySqlConnection OpenConnect()
        {
            try
            {
                if (this.connect.State == System.Data.ConnectionState.Closed)
                {
                    this.connect.Open();
                }
            }
            catch (MySqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }

            return this.connect;
        }

        protected void CloseConnection()
        {
            try
            {
                if (this.connect.State == System.Data.ConnectionState.Open)
                {
                    this.connect.Close();
                }
            }
            catch (MySqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }
        }
    }
}
