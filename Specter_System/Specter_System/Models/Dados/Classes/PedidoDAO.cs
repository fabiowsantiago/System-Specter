using Specter_System.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Specter_System.Models.Dados.Classes
{
    public class PedidoDAO : ConnectionSQLServer
    {
        private SqlDataReader dtReader;

        protected bool Insert(Pedido model)
        {
            bool resp = true;
            int codPessoa = this.PesquisarCodPessoa(model);
            int codCurso = this.PesquisarCodCurso(model);
            SqlCommand command = new SqlCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("_pessoa_cod", codPessoa);
            command.Parameters.AddWithValue("_curso_cod", codCurso);
            command.Parameters.AddWithValue("_data", model.Data);
            command.Parameters.AddWithValue("_horario", model.Horario);
            command.Parameters.AddWithValue("_valor_original", model.Produto.Valor);
            command.Parameters.AddWithValue("_valor_pago", model.ValorPagamento);
            command.Parameters.AddWithValue("_tipo_inscricao", model.TipoInscricao);

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

        protected List<Pedido> Pesquisar_Pedidos_Clientes(Pedido model)
        {
            List<Pedido> listPedidos = null;
            SqlCommand command = new SqlCommand("Select_Pedidos_Pessoa", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("", model.Pessoa.Nome);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    Pedido pedido = new Pedido()
                    {
                        Data = this.dtReader["data"].ToString(),
                        Horario = this.dtReader["horario"].ToString(),
                        ValorPagamento = this.dtReader["valor_pago"].ToString()
                    };
                }
            }
            catch(SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }

            return listPedidos;
        }

        private int PesquisarCodCurso(Pedido model)
        {
            int cod = 0;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT cod FROM curso WHERE nome = @nome";

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
            command.CommandText = "SELECT cod FROM pessoa WHERE nome = @nome";

            command.Parameters.AddWithValue("@nome", model.Pessoa.Nome);

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
