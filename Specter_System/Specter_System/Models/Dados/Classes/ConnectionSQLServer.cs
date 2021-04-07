using System;
using System.Data.SqlClient;

namespace Specter_System.Models.Dados.Classes
{
    public class ConnectionSQLServer
    {
        private SqlConnection connection;

        protected ConnectionSQLServer()
        {
            this.connection = new SqlConnection();
            this.connection.ConnectionString = "Persist Security Info=False;Integrated Security=true;Initial Catalog=expertnutri;server=DESKTOP-9VR0KPK\\MSSQLSERVER02";

            //Persist Security Info=False;User Id=expertnutri;Password=$$Ncrpln2018;Initial Catalog=expertnutri;server=den1.mssql8.gear.host"
            //Persist Security Info=False;Integrated Security=true;Initial Catalog=expertnutri;server=DESKTOP-9VR0KPK\\MSSQLSERVER02"
            //Server=vb800;Database=expertnutri;User Id=fabiowsantiago_SQLLogin_1;Password=$$Ncrpln2018$$
        }

        protected SqlConnection OpenConnection()
        {
            try
            {
                if (this.connection.State == System.Data.ConnectionState.Closed)
                {
                    this.connection.Open();
                }
            }
            catch (SqlException error)
            {
                throw new Exception($"Erro! {error.Message}");
            }

            return this.connection;
        }

        protected void ClosedConnection()
        {
            try
            {
                if (this.connection.State == System.Data.ConnectionState.Open)
                {
                    this.connection.Close();
                }
            }
            catch (SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }

        }
    }
}
