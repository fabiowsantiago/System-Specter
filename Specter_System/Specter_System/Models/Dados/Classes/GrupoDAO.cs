using Specter_System.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Specter_System.Models.Dados.Classes
{
    public class GrupoDAO : ConnectionSQLServer
    {
        private SqlDataReader dtReader;

        protected bool Insert(Grupo model)
        {
            bool resp = false;
            int codPessoa = this.PesquisarCodPessoa(model.Pessoa);
            int codCurso = this.PesquisarCodCurso(model.Produto);

            SqlCommand command = new SqlCommand("Insert_Grupo", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("produto_cod", codCurso);
            command.Parameters.AddWithValue("pessoas_cod", codPessoa);
            command.Parameters.AddWithValue("nome", model.Nome);
            command.Parameters.AddWithValue("senha", model.Senha);
            command.Parameters.AddWithValue("qtd_total", model.QtdComponentes);

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

        protected bool PesquisarNomeGrupo(Grupo model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand("Select_Name_Group", this.OpenConnection());
            command.CommandText = "SELECT nome FROM grupos WHERE nome = @nome";
            
            command.Parameters.AddWithValue("nome", model.Nome);

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

        protected bool Integrar(Grupo model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand("Integrar_Grupo", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("_nome", model.Nome);
            command.Parameters.AddWithValue("_senha", model.Senha);

            try
            {
                this.dtReader = command.ExecuteReader();
                resp = true;
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

        protected Grupo PesquisarQuantidade(Grupo model)
        {
            Grupo grupo = null;
            SqlCommand command = new SqlCommand("Select_Grupo", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("_nome", model.Nome);
            command.Parameters.AddWithValue("_senha", model.Senha);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    grupo = new Grupo()
                    {
                        Nome = this.dtReader["nome"].ToString(),
                        QtdIntegrantes = Convert.ToInt32(this.dtReader["qtdTotal"].ToString()),
                        QtdComponentes = Convert.ToInt32(this.dtReader["qtdIntegrantes"].ToString())
                    };
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

            return grupo;
        }


        protected Grupo SelectGrupo()
        {
            Grupo grupo = null;
            List<Grupo> listGrupos = new List<Grupo>();

            SqlCommand command = new SqlCommand("List_Nome_Grupo", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    listGrupos.Add(
                                   new Grupo()
                                   {
                                       Nome = this.dtReader["nome"].ToString()
                                   }
                    );

                }

                grupo = new Grupo
                {
                    Grupos = listGrupos
                };

                return grupo;
            }
            catch (SqlException error)
            {
                throw new Exception("Erro! " + error.Message);
            }
            finally
            {
                this.dtReader.Close();
                this.ClosedConnection();
            }
        }

        private int PesquisarCodPessoa(string nome)
        {
            int cod = 0;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT cod FROM pessoas WHERE nome = @nome";

            command.Parameters.AddWithValue("@nome", nome);

            try
            {
                command.Connection = this.OpenConnection();
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    cod = Convert.ToInt32(this.dtReader["cod"].ToString());
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

            return cod;
        }

        private int PesquisarCodCurso(Produto model)
        {
            int cod = 0;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT cod FROM produtos WHERE nome = @nome";

            command.Parameters.AddWithValue("@nome", model.Nome);

            try
            {
                command.Connection = this.OpenConnection();
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    cod = Convert.ToInt32(this.dtReader["cod"].ToString());
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

            return cod;
        }

    }
}
