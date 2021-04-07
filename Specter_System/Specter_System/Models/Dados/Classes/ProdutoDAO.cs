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

        public bool Insert_Produtos_Online(Produto model)
        {
            bool resp = false;
            bool respImagem = false;
            bool respVideo = false;

            SqlCommand command = new SqlCommand("Insert_Produto_Online", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("nome", model.Nome);
            command.Parameters.AddWithValue("carga_horaria", model.Carga_Horaria);
            command.Parameters.AddWithValue("valor", model.Valor);
            command.Parameters.AddWithValue("modalidade", "Online");

            try
            {
                command.ExecuteNonQuery();

                respImagem = this.Insert_Imagem_Online(model);

                if (respImagem == true)
                {
                    respVideo = this.Insert_Video(model);

                    if (respVideo == true)
                        resp = true;
                }
                   
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

        private bool Insert_Imagem_Online(Produto model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand("Insert_Imagem_Online", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("nomeProduto", model.Nome);
            command.Parameters.AddWithValue("nomeImagem", model.image.NomeCurso);
            command.Parameters.AddWithValue("caminhoImagem", model.image.CaminhoCurso);

            try
            {
                command.ExecuteNonQuery();

                resp = true;
            }
            catch(Exception error)
            {
                throw new Exception($"Error! {error.Message}");
            }
            finally
            {
                this.ClosedConnection();
            }

            return resp;
        }

        private bool Insert_Video(Produto model)
        {
            bool resp = false;
            int codCurso = this.Pesquisar_Cod_Curso(model);

            SqlCommand commandVideo = new SqlCommand("Insert_Videos",this.OpenConnection());
           
            commandVideo.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
              
                for (int i = 0; i < model.ListVideos.Count; i++)
                {
                    commandVideo.Parameters.AddWithValue("nomeCurso", model.Nome);
                    commandVideo.Parameters.AddWithValue("nome", model.ListVideos[i].Nome);
                    commandVideo.Parameters.AddWithValue("caminho", model.ListVideos[i].Caminho);

                    commandVideo.ExecuteNonQuery();

                    commandVideo.Parameters.Clear();
                }

                resp = true;
            }
            catch (SqlException error)
            {
                throw new Exception($"Error Video{error.Message}");
            }
            finally
            {
                this.ClosedConnection();
            }

            return resp;
        }

        protected Produto Select_Videos(Produto model)
        {
            Produto produto = null;
            List<Video> videos = new List<Video>();
            SqlCommand command = new SqlCommand("Select_Videos", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("nomeProduto", model.Nome);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    videos.Add(
                                new Video()
                                {
                                    Nome = this.dtReader["nome"].ToString(),
                                    Caminho = System.Configuration.ConfigurationManager.AppSettings["caminhoVideos"].Replace(@"\", "/") + "/" + this.dtReader["caminho"]
                                }
                              );
                    produto = new Produto()
                    {
                        ListVideos = videos
                    };
                }
            }
            catch(SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }

            return produto;
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

        //Metodo para listar os cursos Online
        protected List<Produto> Select_Produtos_Online()
        {
            List<Produto> produtos = new List<Produto>();
            SqlCommand command = new SqlCommand("Select_Produto_Online",this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    Imagem imagem = new Imagem()
                    {
                        NomeCurso = this.dtReader["nomeProduto"].ToString(),
                        CaminhoCurso = System.Configuration.ConfigurationManager.AppSettings["caminhoCursoOnline"].Replace(@"\", "/") + "/" + this.dtReader["nomeProduto"].ToString()
                    };

                    produtos.Add(
                        
                        new Produto()
                        {
                            Nome = this.dtReader["nome"].ToString(),
                            Carga_Horaria = this.dtReader["carga_horaria"].ToString(),
                            image = imagem
                        }
                    );
                }

            }
            catch(SqlException error)
            {
                throw new Exception($"ERROR! {error.Message}");
            }
            return produtos;
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
                        image = imagem,
                        //CurriculoPalestrante = this.dtReader["curriculoPalestrante"].ToString()
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
                            CaminhoPalestrante = System.Configuration.ConfigurationManager.AppSettings["caminhoPalestrante"].Replace(@"\", "/") + "/" + this.dtReader["nomePalestrante"],
                           
                        },
                        CurriculoPalestrante = this.dtReader["curriculoPalestrante"].ToString()
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

        protected Produto Select_Sobre_Produto_Online(Produto model)
        {
            Produto prod = null;
            Imagem img = null;
            List<Modulo> modulos = new List<Modulo>();
            SqlCommand command = new SqlCommand("Select_Sobre_Produtos_Online", this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("nome", model.Nome);

            try
            {
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    img = new Imagem()
                    {
                        CaminhoCurso = System.Configuration.ConfigurationManager.AppSettings["caminhoCursoOnline"].Replace(@"\", "/") + "/" + this.dtReader["nomeProduto"].ToString()
                    };

                    modulos.Add(
                        new Modulo()
                        {
                            Nome = this.dtReader["nomeModulos"].ToString()
                        }
                    );

                    prod = new Produto()
                    {
                        Nome = this.dtReader["nome"].ToString(),
                        Carga_Horaria = this.dtReader["carga_horaria"].ToString(),
                        Valor = Convert.ToDecimal(this.dtReader["valor"].ToString()),
                        Informacoes = this.dtReader["informacoes"].ToString(),
                        Sobre = this.dtReader["sobre"].ToString(),
                        image = img,
                        Modulos = modulos
                    };
                }

            }
            catch(SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }

            return prod;
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
           Produto produto = null;
            
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT qtd_vendidos,qtd_disponiveis FROM produtos WHERE nome = @nome";
            command.Parameters.AddWithValue("nome", model.Nome);

            try
            {
                command.Connection = this.OpenConnection();
                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    produto = new Produto()
                    {
                        QtdVendidos = Convert.ToInt32(this.dtReader["qtd_vendidos"].ToString()),
                        QtdDisponiveis = Convert.ToInt32(this.dtReader["qtd_disponiveis"].ToString())
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

            return produto;
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

        public string Select_Link(Produto model)
        {
            string link = string.Empty;
            SqlCommand command = new SqlCommand();
            command.CommandText = "SELECT curriculoPalestrante FROM produtos WHERE palestrante = @palestrante";

            command.Parameters.AddWithValue("@palestrante", model.Palestrante);

            try
            {
                command.Connection = this.OpenConnection();

                this.dtReader = command.ExecuteReader();

                while (this.dtReader.Read())
                {
                    link = this.dtReader["curriculoPalestrante"].ToString();
                }
            }
            catch(SqlException error)
            {
                throw new Exception($"Error! {error.Message}");
            }

            return link;
        }

        public bool UpdateDisponibilidade(Produto model)
        {
            bool resp = false;
            SqlCommand command = new SqlCommand("Update_Disponibilidade",this.OpenConnection());
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("codigo",model.Codigo);
            command.Parameters.AddWithValue("qtdVendidos", model.QtdVendidos);
            command.Parameters.AddWithValue("qtdDisponiveis", model.QtdDisponiveis);

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
                this.OpenConnection();
            }

            return resp;
        }

    }
}
