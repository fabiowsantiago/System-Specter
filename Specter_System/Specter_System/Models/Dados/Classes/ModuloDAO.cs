using Specter_System.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Specter_System.Models.Dados.Classes
{
    public class ModuloDAO : ConnectionSQLServer
    {
        private SqlDataReader dtReader;

        protected bool Insert_Modulo(Produto model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand("Insert_Modulo", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                for (int i = 0; i < model.Modulos.Count; i++)
                {
                    command.Parameters.AddWithValue("nomeProduto", model.Nome);
                    command.Parameters.AddWithValue("nomeModulo", model.Modulos[i].Nome);

                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                };

                resp = true;
            }
            catch (Exception error)
            {
                throw new Exception($"Error! {error.Message}");
            }
            finally
            {
                this.ClosedConnection();
            }

            return resp;
        }

        protected List<Modulo> List_Modulos(Produto model)
        {
            List<Modulo> modulos = new List<Modulo>();
            SqlCommand command = new SqlCommand("Select_Modulos", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {

            }
            catch(SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }

            return modulos;
        }
    }
}
