using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Business;
using Specter_System.Models.Servicos.Infaces;
using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Xml;
using System.Text;

namespace Specter_System.Controllers
{
    public class AreaAlunoController : Controller
    {

        // private INPedido appInscricao = new AppBusinessPedido();
        private INGrupo appGrupo = new AppBusinessGrupo();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AlterarSenha()
        {
            return View();
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
            string qdtComponentes = Request.Form["QtdIntegrantes"].ToString();
            var produto = Session["sessionProduto"] as Produto;
            
            Grupo grupo = new Grupo()
            {
                Nome = model.Grupo.Nome,
                Senha = model.Grupo.Senha,
                ConfSenha = model.Grupo.ConfSenha,
                OpGrupo = "Criar",
                Pessoa = Session["nomePessoa"].ToString(),
                QtdComponentes = Convert.ToInt32(qdtComponentes),
                Produto = Session["sessionProduto"] as Produto
            };

            ViewBag.Message = this.appGrupo.Cadastrar(grupo);

            if ("Já existe um grupo com este nome".Equals(ViewBag.Message))
            {
                Session["sessionNomeProduto"] = produto.Nome;
                Session["sessionValorProduto"] = Convert.ToDecimal(produto.Valor);

                model = new Pedido()
                {
                    Grupo = grupo,
                    Produto = Session["sessionProduto"] as Produto
                };

            }

            else if ("".Equals(ViewBag.Message))
            {
                Session["sessionNomeProduto"] = produto.Nome;
                Session["sessionValorProduto"] = produto.Valor * 100;
                return RedirectToAction("PagSeguro");
            }

            return View(model);

        }
        public ActionResult PagSeguro()
        {
            return View();
        }

        public ActionResult ConsultarPagamento()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Checkout(string txtDescricao)
        {
            Produto produto = Session["sessionProduto"] as Produto;
            //URI de checkout.
            string uri = @"https://ws.pagseguro.uol.com.br/v2/checkout";

            //Conjunto de parâmetros/formData.
            System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
            postData.Add("email", "contatoexpertnutri@gmail.com");
            postData.Add("token", "bc3d065e-db52-48f7-9ce1-104ceb4297c2933005bf46dba229b0f79900ed8c1a938457-31c0-41d5-97e9-25856f6a31b8");
            postData.Add("currency", "BRL");
            postData.Add("itemId1", "0001");
            postData.Add("itemDescription1", produto.Nome);
            postData.Add("itemAmount1", produto.Valor+".00");
            postData.Add("itemQuantity1", "1");
            postData.Add("itemWeight1", "200");
            postData.Add("reference", "REF1234");
            postData.Add("senderName",  "Fabio Welber Santiago");
            postData.Add("senderAreaCode", "44");
            postData.Add("senderPhone", "999999999");
            postData.Add("senderEmail", "comprador@uol.com.br");
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
