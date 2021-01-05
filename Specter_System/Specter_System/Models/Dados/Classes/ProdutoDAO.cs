using Specter_System.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace Specter_System.Models.Dados.Classes
{
    public class ProdutoDAO : ConnectionSQLServer
    {
        private SqlDataReader dtReader;

        protected bool Insert(Produto produto)
        {

            bool resp;
            DateTime date = new DateTime();

            date = Convert.ToDateTime(produto.Data);

            SqlCommand command = new SqlCommand("Insert_Produto", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@nome", produto.Nome);
            command.Parameters.AddWithValue("@data", date);
            command.Parameters.AddWithValue("@horario", produto.Horario);
            command.Parameters.AddWithValue("@carga_horaria", produto.Carga_Horaria);
            command.Parameters.AddWithValue("@palestrante", produto.Palestrante);
            command.Parameters.AddWithValue("@modalidade", produto.Modalidade);
            command.Parameters.AddWithValue("@sobre", produto.Sobre);
            command.Parameters.AddWithValue("@informacoes", produto.Informacoes);
            command.Parameters.AddWithValue("@valor", produto.Valor);
            command.Parameters.AddWithValue("@qtdVagas", produto.QuantidadeDeVagas);
            command.Parameters.AddWithValue("@qtdVendidos", produto.QtdVendidos);
            command.Parameters.AddWithValue("@qtdDisponiveis", produto.QtdDisponiveis);

            try
            {
                command.ExecuteNonQuery();
                resp = true;

            }
            catch (SqlException error)
            {
                throw new Exception("Erro ao cadastrar curso! " + error.Message);
            }
            finally
            {
                this.ClosedConnection();
            }

            if (resp == true)
            {
                resp = this.Cadastrar_Imagem(produto);
            }

            return resp; ;
        }

        private bool Cadastrar_Imagem(Produto model)
        {
            bool resp;
            int codCurso = this.Pesquisar_Cod_Curso(model);
            SqlCommand command = new SqlCommand("Insert_Imagem", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("codProduto", codCurso);
            command.Parameters.AddWithValue("nomeProduto", model.image.NomeCurso);
            command.Parameters.AddWithValue("caminhoProduto", model.image.CaminhoCurso);
            command.Parameters.AddWithValue("nomePalestrante", model.image.NomePalestrante);
            command.Parameters.AddWithValue("caminhoPalestrante", model.image.CaminhoPalestrante);

            try
            {
                command.ExecuteNonQuery();

                resp = true;
            }
            catch (SqlException error)
            {
                throw new Exception("Erro ao cadastrar Imagem! " + error.Message);
            }
            finally
            {
                this.dtReader.Close();
                this.ClosedConnection();
            }

            return resp;
        }

        //Método para pesquisar os cursos por modalidades e exibir na tela Principal
        protected List<Produto> Listar_Cursos_Abertos(Produto model)
        {
            SqlCommand command = new SqlCommand("Select_Produtos_Disponiveis", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("modalidade", model.Modalidade);

            List<Produto> cursos = new List<Produto>();

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    Imagem imagem = null;

                    if ("Presencial".Equals(model.Modalidade))
                    {
                        imagem = new Imagem()
                        {
                            CaminhoCurso = System.Configuration.ConfigurationManager.AppSettings["caminhoCursoPresencial"].Replace(@"\", "/") + "/" + this.dtReader["nomeProduto"],
                            CaminhoPalestrante = System.Configuration.ConfigurationManager.AppSettings["caminhoPalestrante"].Replace(@"\", "/") + "/" + this.dtReader["nomePalestrante"]
                        };
                    }
                    else
                    {
                        imagem = new Imagem()
                        {
                            CaminhoCurso = System.Configuration.ConfigurationManager.AppSettings["caminhoCursoOnline"].Replace(@"\", "/") + "/" + this.dtReader["nomeProduto"],
                            CaminhoPalestrante = System.Configuration.ConfigurationManager.AppSettings["caminhoPalestrante"].Replace(@"\", "/") + "/" + this.dtReader["nomePalestrante"]
                        };
                    }

                    cursos.Add(
                         new Produto()
                         {
                             CursoId = int.Parse(this.dtReader["cod"].ToString()),
                             Nome = this.dtReader["nome"].ToString(),
                             Modalidade = this.dtReader["modalidade"].ToString(),
                             Data = System.DateTime.Parse(this.dtReader["data"].ToString()),
                             Carga_Horaria = this.dtReader["carga_horaria"].ToString(),
                             Valor = decimal.Parse(this.dtReader["valor"].ToString()),
                             image = imagem
                         }); ;
                }

                return cursos;
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
        }

        //Método para filtrar cursos por MODALIDADE, para apresentar na tela Administrativa
        protected List<Produto> PesquisarCursoPorModalidade(Produto curso)
        {
            List<Produto> cursos = new List<Produto>();

            SqlCommand command = new SqlCommand("Select_Produtos_Modalidade", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("modalidade", curso.Modalidade);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    Imagem imagem = null;

                    if ("Online".Equals(curso.Modalidade))
                    {
                        imagem = new Imagem()
                        {
                            CaminhoCurso = System.Configuration.ConfigurationManager.AppSettings["caminhoCursoOnline"].Replace(@"\", "/") + "/" + this.dtReader["nomeProduto"]

                        };
                    }
                    else
                    {
                        imagem = new Imagem()
                        {
                            CaminhoCurso = System.Configuration.ConfigurationManager.AppSettings["caminhoCursoPresencial"].Replace(@"\", "/") + "/" + this.dtReader["nomeProduto"]
                        };
                    }

                    Produto course = new Produto()
                    {
                        CursoId = Int32.Parse(this.dtReader["cod"].ToString()),
                        Nome = this.dtReader["nome"].ToString(),
                        Modalidade = this.dtReader["modalidade"].ToString(),
                        Data = Convert.ToDateTime(this.dtReader["data"].ToString()),
                        Horario = this.dtReader["horario"].ToString(),
                        Carga_Horaria = this.dtReader["carga_horaria"].ToString(),
                        Valor = decimal.Parse(this.dtReader["valor"].ToString()),
                        image = imagem
                    };

                    cursos.Add(course);
                }

                return cursos;
            }
            catch (Exception error)
            {
                throw new Exception($"Erro! {error.Message}");
            }
            finally
            {
                this.ClosedConnection();
                this.dtReader.Close();
            }
        }

        protected Produto Pesquisar_Curso(Produto curso)
        {
            Produto course = null;
            SqlCommand command = new SqlCommand("Select_Produto", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("nome", curso.Nome);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    course = new Produto()
                    {
                        Codigo = Convert.ToInt32(this.dtReader["cod"].ToString()),
                        Nome = this.dtReader["nome"].ToString(),
                        //DataConvert = this.dtReader["data"].ToString(),
                        Data = Convert.ToDateTime(this.dtReader["data"].ToString()),
                        Horario = this.dtReader["horario"].ToString(),
                        Carga_Horaria = this.dtReader["carga_horaria"].ToString(),
                        Palestrante = this.dtReader["palestrante"].ToString(),
                        Modalidade = this.dtReader["modalidade"].ToString(),
                        Informacoes = this.dtReader["informacoes"].ToString(),
                        Sobre = this.dtReader["sobre"].ToString(),
                        Valor = decimal.Parse(this.dtReader["valor"].ToString()),
                        image = new Imagem()
                        {
                            CaminhoPalestrante = System.Configuration.ConfigurationManager.AppSettings["caminhoPalestrante"].Replace(@"\", "/") + "/" + this.dtReader["nomePalestrante"]
                        }
                    };
                }

                return course;
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
        }

        //VERIFICAR PARA EXCLUIR POIS O MÉTODO PESQUISAR CURSO FAZ O MESMO RECURSO
        protected List<Produto> Pesquisar_Cursos(Produto curso)
        {
            List<Produto> listCursos = new List<Produto>();
            SqlCommand command = new SqlCommand("Select_Produtos", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("_nome", curso.Nome);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    curso = new Produto()
                    {
                        CursoId = Int32.Parse(this.dtReader["cod"].ToString()),
                        Data = DateTime.ParseExact(this.dtReader["data"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Horario = this.dtReader["horario"].ToString(),
                        Carga_Horaria = this.dtReader["carga_horaria"].ToString(),
                        Palestrante = this.dtReader["palestrante"].ToString(),
                        Modalidade = this.dtReader["modalidade"].ToString(),
                        Informacoes = this.dtReader["Informacoes"].ToString(),
                        Valor = decimal.Parse(this.dtReader["valor"].ToString())
                    };

                    listCursos.Add(curso);
                }

                return listCursos;
            }
            catch (SqlException error)
            {
                throw new Exception($"Erro! {error.Message}");
            }
        }

        protected Produto PesquisarVagasDisponiveis(Produto model)
        {
            Produto curso = null;
            SqlCommand command = new SqlCommand("Select_Quantidade_De_Inscritos", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("nome", model.Nome);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    curso = new Produto()
                    {
                        QuantidadeDeVagas = Convert.ToInt32(this.dtReader["qtd_vagas"].ToString()),
                        QtdVendidos = Convert.ToInt32(this.dtReader["qtd_vendidos"].ToString())
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

            return curso;
        }

        protected bool Alterar_Curso(Produto curso)
        {
            bool resp;
            SqlCommand command = new SqlCommand("Update_Curso", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("_cod", curso.CursoId);
            command.Parameters.AddWithValue("_nome", curso.Nome);
            command.Parameters.AddWithValue("_data", curso.Data);
            command.Parameters.AddWithValue("_horario", curso.Horario);
            command.Parameters.AddWithValue("_cargaHoraria", curso.Carga_Horaria);
            command.Parameters.AddWithValue("_palestrante", curso.Palestrante);
            command.Parameters.AddWithValue("_modalidade", curso.Modalidade);
            command.Parameters.AddWithValue("_informacoes", curso.Informacoes);
            command.Parameters.AddWithValue("_valor", curso.Valor);

            try
            {
                command.ExecuteNonQuery();

                resp = true;
            }
            catch (SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }
            return resp;
        }

        protected bool Excluir_Curso(Produto curso)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand("Delete_Curso", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("_cod", curso.Codigo);

            try
            {
                command.ExecuteReader();

                resp = true;
            }
            catch (Exception error)
            {
                throw new Exception("Error! " + error.Message);
            }
            finally
            {
                this.ClosedConnection();
            }

            return resp;
        }

        private int Pesquisar_Cod_Curso(Produto curso)
        {
            int cod = 0;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT cod FROM produtos WHERE nome = @nome";
            
            command.Parameters.AddWithValue("nome", curso.Nome);

            try
            {
                command.Connection = this.OpenConnection();
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    cod = Convert.ToInt32(this.dtReader["cod"].ToString());
                }

                return cod;
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
        }

    }
}
