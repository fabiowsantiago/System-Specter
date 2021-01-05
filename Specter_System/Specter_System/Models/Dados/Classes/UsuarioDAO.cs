using MySql.Data.MySqlClient;
using Specter_System.Models.Entitys;
using System;
using System.Data.SqlClient;

namespace Specter_System.Models.Dados.Classes
{
    public class UsuarioDAO : ConnectionSQLServer
    {
        private SqlDataReader dtReader;

        protected Usuario ValidarLogin(Usuario model)
        {
            Usuario user = null;
            SqlCommand command = new SqlCommand("Validar_Login", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("email", model.Email);
            command.Parameters.AddWithValue("senha", model.Senha);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    user = new Usuario()
                    {
                        Pessoa = this.dtReader["nome"].ToString(),
                        Email = this.dtReader["email"].ToString(),
                        Senha = this.dtReader["senha"].ToString(),
                        Perfil = this.dtReader["perfil"].ToString()
                    };
                }
            }
            catch (MySqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }
            finally
            {
                this.dtReader.Close();
                this.ClosedConnection();
            }

            return user;
        }

        protected bool InsertUsuario(Usuario model)
        {
            int codPessoa = this.SelectCodPessoa(model);
            bool resp = false;
            SqlCommand command = new SqlCommand("Insert_Usuario",this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("codPessoa", codPessoa);
            command.Parameters.AddWithValue("email", model.Email);
            command.Parameters.AddWithValue("senha", model.Senha);
            command.Parameters.AddWithValue("perfil", model.Perfil);

            try
            {
                command.ExecuteNonQuery();

                resp = true;
            }
            catch(SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }
            finally
            {
                this.ClosedConnection();
            }

            return resp;
        }

        //Método para verificar se o E-MAIL já encontra-se cadastrado no banco
        protected bool VerificarEmail(Usuario model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT email FROM usuario WHERE email = @email";

            command.Parameters.AddWithValue("@email", model.Email);

            try
            {
                command.Connection = this.OpenConnection();

                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    resp = true;
                }

            }
            catch(SqlException error)
            {
                throw new Exception($"ERROR! {error.Message}");
            }
            finally
            {
                this.dtReader.Close();
                this.ClosedConnection();
            }

            return resp;
        }
        
        private int SelectCodPessoa(Usuario model)
        {
            int cod = 0;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT cod FROM pessoa WHERE nome = @nome";

            command.Parameters.AddWithValue("nome", model.Pessoa);

            try
            {
                command.Connection = this.OpenConnection();

                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    cod = Convert.ToInt32(this.dtReader["cod"].ToString());
                }
            }
            catch(SqlException error)
            {
                throw new Exception($"ERROR! {error.Message}");
            }
            finally
            {
                this.dtReader.Close();
                this.ClosedConnection();
            }
            return cod;
        }
    }
}
