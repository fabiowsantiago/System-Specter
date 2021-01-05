using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Business;
using Specter_System.Models.Servicos.Infaces;
using System;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml;

namespace Specter_System.Controllers
{
    public class InscricaoController : Controller
    {
        private INGrupo appGrupo = new AppBusinessGrupo();

        public ViewResult Index()
        {
            Pedido inscricao = new Pedido()
            {
                Data = DateTime.Now.ToShortDateString(),
                Horario = DateTime.Now.ToShortTimeString(),
                Produto = Session["sessionProduto"] as Produto
            };

            return View(inscricao);
        }

        [HttpPost]
        public ViewResult Index(Pedido model)
        {
            string qdtComponentes = Request.Form["QtdIntegrantes"].ToString();

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

            model = new Pedido()
            {
                Resposta = this.appGrupo.Cadastrar(grupo)
            };

           // ViewBag.Message = this.appGrupo.Cadastrar(grupo);

            return View(model);
        }

        [HttpPost]
        [MultipleButton(Name ="Action",Argument ="ViewPagSeguro")]
        public JsonResult ViewPagSeguro(Pedido model)
        {
            string descricao = model.Produto.Nome;
            decimal valor = model.Produto.Valor;
            int quantidade = 1;

            string uri = @"https://ws.pagseguro.uol.com.br/v2/checkout";

            //Conjunto de parâmetros/formData.
            System.Collections.Specialized.NameValueCollection postData = new System.Collections.Specialized.NameValueCollection();
            postData.Add("email", "contatoexpertnutri@gmail.com");
            postData.Add("token", "bc3d065e-db52-48f7-9ce1-104ceb4297c2933005bf46dba229b0f79900ed8c1a938457-31c0-41d5-97e9-25856f6a31b8");
            postData.Add("currency", "BRL");
            postData.Add("itemId1", "0001");
            postData.Add("itemDescription1", descricao);
            postData.Add("itemAmount1", Convert.ToString(valor + ".00"));
            postData.Add("itemQuantity1", Convert.ToString(quantidade));
            postData.Add("itemWeight1", "200");
            postData.Add("reference", "REF1234");
            postData.Add("senderName", "Jose Comprador");
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
    }
}
