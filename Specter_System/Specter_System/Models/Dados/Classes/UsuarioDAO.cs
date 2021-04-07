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
                        Perfil = this.dtReader["perfil"].ToString(),
                        Status = bool.Parse(this.dtReader["status"].ToString())
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

        public bool Update_Cod_Senha(Usuario model) //Método para inserir o código de alteração de senha
        {
            bool resp = false;
            SqlCommand command = new SqlCommand();
            command.CommandText = "Update usuarios SET codSenha = @codSenha WHERE email = @email";

            command.Parameters.AddWithValue("@codSenha", model.codSenha);
            command.Parameters.AddWithValue("@email", model.Email);
         
            try
            {
                command.Connection = this.OpenConnection();

                command.ExecuteNonQuery();

                resp = true;
            }
            catch (SqlException error)
            {
                throw new Exception($"Error{error.Message}");
            }
            finally
            {
                this.ClosedConnection();
            }

            return resp;
        }

        public bool Verificar_Cod_Senha(Usuario model)
        {
            bool resp = false;
            int cod = 0;
            SqlCommand command = new SqlCommand("Select_Cod_Senha",this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@cpf", model.CpfPessoa);
            command.Parameters.AddWithValue("@cod", model.codSenha);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    cod = Convert.ToInt32(this.dtReader["codSenha"].ToString());
                }
            }
            catch(SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }
            finally
            {
                this.dtReader.Close();
                this.ClosedConnection();
            }

            if (cod > 0)
                resp = true;

            return resp;
        }

        public bool Update_Senha(Usuario model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand("Update_Senha",this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@cpf", model.CpfPessoa);
            command.Parameters.AddWithValue("@senha", model.Senha);
     
            try
            {
                command.Connection = this.OpenConnection();

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

        public string Recuperar_Email(Usuario model)
        {
            string email = string.Empty;

            SqlCommand command = new SqlCommand("Select_Email", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("nome", model.Pessoa);
            command.Parameters.AddWithValue("cpf",model.CpfPessoa);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    email = this.dtReader["email"].ToString();
                }

            }
            catch (SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }
            finally
            {
                this.ClosedConnection();
                this.dtReader.Close();
            }

            return email;
        }

       
        //Método para verificar se o E-MAIL já encontra-se cadastrado no banco
        protected bool VerificarEmail(Usuario model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT email FROM usuarios WHERE email = @email";

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

        protected bool BloquearLogin(Usuario model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand();
            command.CommandText = "UPDATE usuarios SET status = @status WHERE email = @email";

            command.Parameters.AddWithValue("@status", model.Status);
            command.Parameters.AddWithValue("@email", model.Email);

            try
            {
                command.Connection = this.OpenConnection();
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
        

        protected bool DesbloquearLogin(Usuario model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand();
            command.CommandText = "UPDATE usuarios SET status = @status WHERE email = @email";

            command.Parameters.AddWithValue("@status", model.Status);
            command.Parameters.AddWithValue("@email", model.Email);

            try
            {
                command.Connection = this.OpenConnection();
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
                
        private int SelectCodPessoa(Usuario model)
        {
            int cod = 0;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT cod FROM pessoas WHERE nome = @nome";

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
