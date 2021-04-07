using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Business;
using Specter_System.Models.Servicos.Infaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace Specter_System.Controllers
{
    public class AreaAlunoController : Controller
    {

        private INPedido appPedido = new AppBusinessPedido();
        private INGrupo appGrupo = new AppBusinessGrupo();
        private INUsuario appUsuario = new AppBusinessUsuario();
        private INCertificado appCertificado = new AppBusinessCertificado();
        private INProduto appProduto = new AppBusinessProduto();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AlterarSenha()
        {
            Usuario model = new Usuario();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultipleButton(Name = "Action", Argument = "AlterarSenha")]
        public ActionResult AlterarSenha(Usuario model)
        {
            return View();
        }

        public ActionResult CarrinhoCompra()
        {
            var produto = Session["sessionProduto"] as Produto;

            return View(produto);
        }

        public ActionResult Inscricao()
        {
            Pedido inscricao = new Pedido()
            {
                Data = DateTime.Now.ToShortDateString(),
                Horario = DateTime.Now.ToShortTimeString(),
                Produto = Session["sessionProduto"] as Produto
            };

            return View(inscricao);
            //return RedirectToAction("../Inscricao/Index");
        }

        [HttpPost]
        [MultipleButton(Name = "Action", Argument = "Inscricao")]
        public ActionResult Inscricao(Pedido model)
        {
            int qdtComponentes = Convert.ToInt32(Request.Form["QtdIntegrantes"].ToString());
            double valorProduto = Convert.ToDouble(Session["sessionValorProduto"].ToString()); //Sessiona criada na classe CursosPresenciaisController
            var produto = Session["sessionProduto"] as Produto;
            
            Grupo grupo = new Grupo()
            {
                Nome = model.Grupo.Nome,
                Senha = model.Grupo.Senha,
                ConfSenha = model.Grupo.ConfSenha,
                OpGrupo = "Criar",
                Pessoa = Session["nomePessoa"].ToString(),
                QtdComponentes = qdtComponentes,
                Produto = Session["sessionProduto"] as Produto
            };

            Pedido pedido = new Pedido()
            {
                Data = Convert.ToString(DateTime.Now.Date),
                Horario = Convert.ToString(DateTime.Now.Hour),
                Valor_Pedido = Convert.ToDecimal(valorProduto * qdtComponentes),
                ValorPagamento = Convert.ToDecimal(this.CacularValorPagar(valorProduto, qdtComponentes)),
                TipoInscricao = Request.Form["tipoInscricao"].ToString(),
                Produto = produto,
                Grupo = grupo,
                Status = "Pendente",
                Pessoa = Session["nomePessoa"].ToString()
            };

            Session["sessionPedido"] = pedido; //Criado uma SESSION de pedido para inserir os valor pagto no PAGSEGURO 

            ViewBag.Message = this.appPedido.RealizarIncricao(pedido);

            if (!"Pre inscricao realizado com sucesso".Equals(ViewBag.Message)) //Cria um objeto carregar os dados na View
            {
                Session["sessionNomeProduto"] = produto.Nome;
                Session["sessionValorProduto"] = Convert.ToDecimal(produto.Valor);

                model = new Pedido()
                {
                    Grupo = grupo,
                    Produto = Session["sessionProduto"] as Produto
                };
            }
            else
            {
                Session["sessionNomeProduto"] = produto.Nome;
                Session["sessionValorProduto"] = produto.Valor * 100;

                model = pedido;
            }

            return View(model);

        }

        public ActionResult Sair()
        {
            Usuario user = new Usuario()
            {
                Email = Session["usuarioLogado"].ToString(),
                Status = false
            };

            this.appUsuario.SairLogin(user);
            
            return RedirectToAction("../Home/Index");
        }

        public ActionResult ConsultarPagamento()
        {
            return View();
        }

        public ActionResult CursosOnline()
        {
            Pedido pedido = new Pedido()
            {
                Pessoa = "Fabio Welber Santiago" //Session["nomePessoa"].ToString()
            };

            Pedidos model = this.appPedido.Pesquisar_Pedidos_Online(pedido);
           
            return View(model);
        }

        public ActionResult ExibirCursosOnlines(Produto model)
        {
            Produto produto = new Produto()
            {
                Nome = model.Nome
            };

            produto = this.appProduto.PesquisarVideos(model);

            return View(produto);
        }

        public ActionResult ExibirCertificados()
        {

            List<Certificado> certificados = new List<Certificado>();
            certificados.Add(
                new Certificado()
                {
                    Curso = "Desenvolvimento Web",
                    Data = "01/01/2021",
                    Horario = "08:00",
                    CargaHoraria = "120 hs"
                }
            );

            Certificado certificado = new Certificado() { Certificados = certificados};
           

              return View(certificado);
        }
               
        public ActionResult EmitirCertificado(Certificado model)
        {
            Certificado certificado = new Certificado()
            {
                Curso = "Desenvolvimento Web",
                Data = "12/02/2020",
                CargaHoraria = "140 hs",
                Palestrante = "Emanuelle Cruz da Silva Santiago"
                
            };

            return View(certificado);
        }
        
        [HttpPost]
        [MultipleButton(Name ="action", Argument= "ImprimirCertificado")]
        public ActionResult ImprimirCertificado(Certificado model)
        {

            Certificado certificado = new Certificado()
            {
                Curso = model.Curso,
                Data = model.Data,
                CargaHoraria = model.CargaHoraria,
                Palestrante = "Emanuellee Cruz da Silva Santiago",
                Aluno = "Fabio Welber Santiago"
            };

            string resp = this.appCertificado.ImprimirCertificado(certificado);

            return RedirectToAction("EmitirCertificado");
        }

        public ActionResult IntegrarGrupo()
        {
            Grupo grupo = this.appGrupo.ListarGrupos();

            List<string> grupos = new List<string>();

            foreach (var model in grupo.Grupos)
            {
                grupos.Add(model.Nome);
            }

            ViewBag.NomeGrupo = new MultiSelectList(grupos, "NomeGrupo");

            return View(grupo);
        }

        private double CacularValorPagar(double valorProduto, int qtdComponentes)
        {
            double valor = 0;
            double total = 0;

            if (qtdComponentes == 2)
            {
                valor = valorProduto * 2;
                total = Math.Round(valor - (valor * 0.02));

            }
            else if (qtdComponentes == 3)
            {
                valor = valorProduto * 3;
                total = Math.Round(valor - (valor * 0.03));
            }
            else if (qtdComponentes == 4)
            {
                valor = valorProduto * 4;
                total = Math.Round(valor - (valor * 0.04));
            }
            else if (qtdComponentes == 5)
            {
                valor = valorProduto * 5;
                total = Math.Round(valor - (valor * 0.05));
            }
            else if(qtdComponentes == 6)
            {
                valor = valorProduto * 6;
                total = Math.Round(valor - (valor * 0.06));
            }

            return total;
        }

        [HttpPost]
        public JsonResult Checkout(string txtDescricao)
        {
            var pedido = Session["sessionPedido"] as Pedido;
            var produto = Session["sessionProduto"] as Produto;
         
            //URI de checkout.
            string uri = @"https://ws.pagseguro.uol.com.br/v2/checkout";

            //Conjunto de parâmetros/formData.
            System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
            postData.Add("email", "contatoexpertnutri@gmail.com");
            postData.Add("token", "bc3d065e-db52-48f7-9ce1-104ceb4297c2933005bf46dba229b0f79900ed8c1a938457-31c0-41d5-97e9-25856f6a31b8");
            postData.Add("currency", "BRL");
            postData.Add("itemId1", Convert.ToString(pedido.Produto.Codigo));
            postData.Add("itemDescription1", pedido.Produto.Nome);
            postData.Add("itemAmount1", pedido.ValorPagamento + ".00");
            postData.Add("itemQuantity1", "1");
            postData.Add("itemWeight1", "200");
            postData.Add("reference", "REF1234"); //Codigo do pedido
            postData.Add("senderName",  pedido.Pessoa);
            postData.Add("senderAreaCode", "44");
            postData.Add("senderPhone", "999999999");
            postData.Add("senderEmail", Session["usuarioLogado"].ToString());
            postData.Add("shippingAddressRequired", "false");

            //String que receberá o XML de retorno.
            string xmlString = null;

            //Webclient faz o post para o servidor de pagseguro.
            using (WebClient wc = new WebClient())
            {
                //Informa header sobre URL.
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                //Faz o POST e retorna o XML contendo resposta do servidor do pagseguro.
                var result = wc.UploadValues(uri, postData);

                //Obtém string do XML.
                xmlString = Encoding.ASCII.GetString(result);
            }

            //Cria documento XML.
            XmlDocument xmlDoc = new XmlDocument();

            //Carrega documento XML por string.
            xmlDoc.LoadXml(xmlString);

            //Obtém código de transação (Checkout).
            var code = xmlDoc.GetElementsByTagName("code")[0];

            //Obtém data de transação (Checkout).
            var date = xmlDoc.GetElementsByTagName("date")[0];

            //Monta a URL para pagamento.
            var paymentUrl = string.Concat("https://pagseguro.uol.com.br/v2/checkout/payment.html?code=", code.InnerText);

            //Retorna dados para HTML.
            return Json(paymentUrl);
        }

        [HttpPost]
        public JsonResult ConsultaTransacao(string codPedido)
        {
            var Resultado = String.Empty;

            //uri de consulta da transação.
            string uri = "https://ws.pagseguro.uol.com.br/v3/transactions/" + codPedido + "?email=contatoexpertnutri@gmail.com&token=bc3d065e-db52-48f7-9ce1-104ceb4297c2933005bf46dba229b0f79900ed8c1a938457-31c0-41d5-97e9-25856f6a31b8";

            //Classe que irá fazer a requisição GET.
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

            //Método do webrequest.
            request.Method = "GET";

            //String que vai armazenar o xml de retorno.
            string xmlString = null;

            //Obtém resposta do servidor.
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                //Cria stream para obter retorno.
                using (Stream dataStream = response.GetResponseStream())
                {
                    //Lê stream.
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        //Xml convertido para string.
                        xmlString = reader.ReadToEnd();

                        //Cria xml document para facilitar acesso ao xml.
                        XmlDocument xmlDoc = new XmlDocument();

                        //Carrega xml document através da string com XML.
                        xmlDoc.LoadXml(xmlString);

                        //Busca elemento status do XML.
                        var status = xmlDoc.GetElementsByTagName("status")[0];

                        //Fecha reader.
                        reader.Close();

                        //Fecha stream.
                        dataStream.Close();

                        //Verifica status de retorno.
                        //3 = Pago.

                        if (status.InnerText == "1")
                        {
                            Resultado = "Aguardando Pagamento.";
                            return Json("Aguardando Pagamento.");

                        }
                        else if (status.InnerText == "2")
                        {
                            return Json("Em análise.");
                        }
                        else if (status.InnerText == "3")
                        {
                            Resultado = "Pagamento efetuado.";
                            return Json("Pagamento efetuado.");
                        }
                        else if (status.InnerText == "6")
                        {
                            return Json("Valor devolvido.");
                        }
                        //Outros resultados diferentes de 3.
                        else if (status.InnerText == "7")
                        {
                            Resultado = "Cancelado.";
                            return Json(Resultado);
                        }
                        else
                        {
                            return Json("Entre em contato com a Instituição");
                        }

                    }
                }
            }
        }
    }
}
