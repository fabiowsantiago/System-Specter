using Specter_System.Models.Dados.Business;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Infaces;

namespace Specter_System.Models.Servicos.Business
{
    public class AppBusinessPedido
    {
        private IPProduto appProduto = new AppPersistenciaProduto();
        private IPGrupo appGrupo = new AppPersistenciaGrupo();

        public string RealizarIncricao(Pedido model)
        {
            Pedido pedido = null;
            string resposta = string.Empty;
            int qtdVagas = 0;

            pedido.Produto = this.appProduto.Pesqusiar_Quantidade_Vagas(model.Produto);

            //Verifica se existe vaga disponivel para o curso
            if (pedido.Produto.QuantidadeDeVagas == pedido.Produto.QtdVendidos)
            {
                resposta = "Vagas indisponiveis";
            }

            else
            {
                qtdVagas = pedido.Produto.QuantidadeDeVagas - pedido.Produto.QtdVendidos;

                if ("Grupo".Equals(model.TipoInscricao))
                {
                    if (qtdVagas < model.Grupo.QtdComponentes) //Verifica se ó tamanho do grupo é maior que a qtd de vagas disponíveis
                    {
                        resposta = $"Existe apenas {qtdVagas} disponíveis";
                    }

                    else if (model.Grupo.Senha != model.Grupo.ConfSenha)
                    {
                        resposta = "Senhas não conferem";
                    }

                    else
                    {
                        bool respGrupo = this.appGrupo.PesquisarGrupo(model.Grupo); //Verifica se grupo já está cadastrado

                        if (respGrupo == true)
                        {
                            resposta = $"Grupo {model.Grupo.Nome} já existe";
                        }
                        else
                        {

                        }
                    }

                }

            }
            return resposta;
        }
    }
}
