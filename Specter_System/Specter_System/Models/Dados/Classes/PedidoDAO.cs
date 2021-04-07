using Specter_System.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Specter_System.Models.Dados.Classes
{
    public class PedidoDAO : ConnectionSQLServer
    {
        private SqlDataReader dtReader;

        protected bool Insert_Grupo(Pedido model)
        {
            bool resp = true;
            int codPessoa = this.PesquisarCodPessoa(model);
            SqlCommand command = new SqlCommand("Insert_Pedido_Grupo", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("codPessoa", codPessoa);
            command.Parameters.AddWithValue("codProduto", model.Produto.Codigo);
            command.Parameters.AddWithValue("data", model.Data);
            command.Parameters.AddWithValue("horario", model.Horario);
            command.Parameters.AddWithValue("valorOriginal", model.Valor_Pedido);
            command.Parameters.AddWithValue("valorPago", model.ValorPagamento);
            command.Parameters.AddWithValue("tipoInscricao", model.TipoInscricao);
            command.Parameters.AddWithValue("tamanhoGrupo", model.Grupo.QtdComponentes);
            command.Parameters.AddWithValue("status", model.Status);

            try
            {
                command.ExecuteNonQuery();
                resp = true;
            }
            catch (SqlException error)
            {
                throw new Exception($"Error! {error.Message} ");
            }

            return resp;
        }

        protected bool Insert_Individual(Pedido model)
        {
            bool resp = true;
            int codPessoa = this.PesquisarCodPessoa(model);
            int codCurso = this.PesquisarCodCurso(model);
            SqlCommand command = new SqlCommand("Insert_Pedido_Individual", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("pessoa_cod", codPessoa);
            command.Parameters.AddWithValue("curso_cod", codCurso);
            command.Parameters.AddWithValue("data", model.Data);
            command.Parameters.AddWithValue("horario", model.Horario);
            command.Parameters.AddWithValue("valor_original", model.Valor_Pedido);
            command.Parameters.AddWithValue("valor_pago", model.ValorPagamento);
            command.Parameters.AddWithValue("tipo_inscricao", model.TipoInscricao);
            command.Parameters.AddWithValue("status", model.Status);

            try
            {
                command.ExecuteNonQuery();
                resp = true;
            }
            catch (SqlException error)
            {
                throw new Exception($"Error! {error.Message} ");
            }

            return resp;
        }

        protected Pedidos Pesquisar_Pedidos_Online(Pedido model)
        {
            Pedidos pedidos = null;
            List<Produto> produtos = new List<Produto>();
            SqlCommand command = new SqlCommand("Select_Pedidos_Online", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("nomePessoa", model.Pessoa);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    Produto produto = new Produto()
                    {
                        Nome = this.dtReader["nome"].ToString(),
                        Carga_Horaria = this.dtReader["carga_horaria"].ToString()
                    };

                    produtos.Add(produto);

                    pedidos = new Pedidos()
                    {
                        Produtos = produtos
                    };

                }
            }
            catch(SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }

            return pedidos;
        }

        private int PesquisarCodCurso(Pedido model)
        {
            int cod = 0;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT cod FROM produtos WHERE nome = @nome";

            command.Parameters.AddWithValue("@nome", model.Produto.Nome);

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
        private int PesquisarCodPessoa(Pedido model)
        {
            int cod = 0;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT cod FROM pessoas WHERE nome = @nome";

            command.Parameters.AddWithValue("@nome", model.Pessoa);

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
