using Specter_System.Models.Dados.Business;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Infaces;
using System;

namespace Specter_System.Models.Servicos.Business
{
    public class AppBusinessPedido : INPedido
    {
        private IPProduto appProduto = new AppPersistenciaProduto();
        private IPGrupo appGrupo = new AppPersistenciaGrupo();
        private IPPedido appPedido = new AppPersistenciaPedido();

        public string RealizarIncricao(Pedido model)
        {
            string resposta = string.Empty;
    
            Produto produto = this.appProduto.Pesqusiar_Quantidade_Vagas(model.Produto);

            //Verifica se existe vaga disponivel para o curso
            if (produto.QtdDisponiveis == 0)
            {
                resposta = "Vagas indisponiveis";
            }

            else
            {
                if ("Grupo".Equals(model.TipoInscricao))
                {
                    if (produto.QtdDisponiveis < model.Grupo.QtdComponentes) //Verifica se ó tamanho do grupo é maior que a qtd de vagas disponíveis
                    {
                        resposta = $"Existe apenas {produto.QtdDisponiveis} disponíveis";
                    }

                    else if (model.Grupo.Senha != model.Grupo.ConfSenha) //Verifica se as senhas são iguais
                    {
                        resposta = "Senhas nao conferem";
                    }

                    else
                    {
                        bool respGrupo = this.appGrupo.PesquisarGrupo(model.Grupo); //Verifica se grupo já está cadastrado

                        if (respGrupo == true)
                        {
                            resposta = "Grupo ja existe";
                        }
                        else
                        {
                           bool respCadastroGrupo = this.appGrupo.Cadastrar(model.Grupo);

                            if(respCadastroGrupo == true) //Verifica se o GRUPO foi cadastrado com sucesso no BANCO
                            {
                                Produto pro = new Produto()
                                {
                                    Codigo = model.Produto.Codigo,
                                    QtdDisponiveis = produto.QtdDisponiveis - model.Grupo.QtdComponentes,
                                    QtdVendidos = produto.QtdVendidos + model.Grupo.QtdComponentes
                                };

                                Pedido pedido = new Pedido()
                                {
                                    Data = model.Data,
                                    Horario = model.Horario,
                                    Valor_Pedido = model.Valor_Pedido,
                                    ValorPagamento = model.ValorPagamento,
                                    Produto = pro,
                                    Grupo = model.Grupo
                                };

                                bool respCadastroInscricao = this.appPedido.Insert_Pedido_Grupo(model);

                                if(respCadastroInscricao == true)
                                {
                                    bool respUpdateProduto = this.appProduto.Update_Disponibilidade(pro);

                                    if(respUpdateProduto == true)
                                    {
                                        resposta = "Pre inscricao realizado com sucesso";
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return resposta;
        }

        public Pedidos Pesquisar_Pedidos_Online(Pedido model)
        {
            Pedidos pedidos = this.appPedido.PesquisarPedidosOnline(model);

            return pedidos;
        }


    }
}
