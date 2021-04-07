using Specter_System.Models.Entitys;
using System;
using System.Data.SqlClient;

namespace Specter_System.Models.Dados.Classes
{
    public class PessoaDAO : ConnectionSQLServer
    {
        private SqlDataReader dtReader;

        protected bool Insert(Pessoa model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand("Insert_Pessoa", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            string nascimento = model.Nascimento.Date.ToString("yyyy-MM-dd");
            
            command.Parameters.AddWithValue("nome", model.Nome);
            command.Parameters.AddWithValue("cpf", model.CPF);
            command.Parameters.AddWithValue("nascimento", nascimento);
            command.Parameters.AddWithValue("profissao", model.Profissao);

            try
            {
                command.ExecuteNonQuery();
                resp = true;
            }
            catch (SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }
            finally
            {
                this.ClosedConnection();
            }

            return resp;
        }

        protected bool PesquisarCpf(Pessoa model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT cpf FROM pessoas WHERE cpf = @cpf";

            command.Parameters.AddWithValue("cpf", model.CPF);

            try
            {
                command.Connection = this.OpenConnection();
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    resp = true;
                }
            }
            catch (SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }
            finally
            {
                this.dtReader.Close();
                this.ClosedConnection();
            }

            return resp;
        }
    }
}
