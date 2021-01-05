using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Business;
using Specter_System.Models.Servicos.Infaces;
using System.Web.Mvc;

namespace Specter_System.Controllers
{
    public class HomeController : Controller
    {
        private INUsuario appUsuario = new AppBusinessUsuario();
        private INPessoa appPessoa = new AppBusinessPessoa();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SobreEmpresa()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [MultipleButtonAttribute(Name = "action", Argument = "Login")]
        public ActionResult Login(Usuario model)
        {
            Usuario user = this.appUsuario.ValidarLogin(model);

            if (user == null)
                ViewBag.Validacao = "Usuario ou senha inválido";
            else
            {
                Session["usuarioLogado"] = user.Email;
                Session["nomePessoa"] = user.Pessoa;

                if ("Administrador".Equals(user.Perfil))
                {
                    return RedirectToAction("AreaAdministrativa");
                }
                else
                {
                    return RedirectToAction("AreaAluno");
                }
            }

            return View();
        }

        public ActionResult AreaAluno()
        {
            if (Session["usuarioLogado"] != null)
                return View("../AreaAluno/Index");
            else
                return RedirectToAction("Login");
        }

        public ActionResult AreaAdministrativa()
        {
            if (Session["usuarioLogado"] != null)
                return View("../AreaAdministrativa/Index");
            else
                return RedirectToAction("Login");
        }

        public ActionResult CadastrarPessoa()
        {
            return View();
        }

        [HttpPost]
        [MultipleButtonAttribute(Name = "action", Argument = "CadastrarPessoa")]
        public ActionResult CadastrarPessoa(Pessoa model)
        {
            Pessoa pessoa = new Pessoa()
            {
                Nome = model.Nome,
                CPF = model.CPF,
                Nascimento = model.Nascimento,
                Profissao = Request.Form["profissao"].ToString()
        };

            string resposta = this.appPessoa.CadastrarPessoa(model);

            if (!"Cadastrado".Equals(resposta))
            {
                ViewBag.Resposta = resposta;
            }
            else
            {
                Session["sessionPessoa"] = model;

                return RedirectToAction("CadastrarUsuario");
            }

            return View();
        }

        public ActionResult CadastrarUsuario()
        {
            var pessoa = Session["sessionPessoa"] as Pessoa;

            Usuario model = new Usuario()
            {
                Pessoa = pessoa.Nome
            };

            return View(model);
        }

        [HttpPost]
        [MultipleButton(Name ="action", Argument ="CadastrarUsuario")]
        public ActionResult CadastrarUsuario(Usuario model)
        {
            var pessoa = Session["sessionPessoa"] as Pessoa; 

            Usuario user = new Usuario()
            {
                Email = model.Email,
                Senha = model.Senha,
                ConfirmarSenha = model.ConfirmarSenha,
                Perfil = "Usuario",
                Pessoa = pessoa.Nome
            };

            string resp = this.appUsuario.Cadastrar_Usuario(user);

            if ("Cadastrado".Equals(resp))
            {
                return RedirectToAction("Login");
            }
            else
            {
                Usuario usuario = new Usuario()
                {
                    Pessoa = pessoa.Nome,
                    Senha = "",
                    ConfirmarSenha = ""
                };

                ViewBag.Message = resp;

                return View(usuario);
            }

        }
    }
}