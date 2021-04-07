using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Business;
using Specter_System.Models.Servicos.Infaces;
using System;
using System.Net.Mail;
using System.Web.Mvc;

namespace Specter_System.Controllers
{
    public class HomeController : Controller
    {
        private INUsuario appUsuario = new AppBusinessUsuario();
        private INPessoa appPessoa = new AppBusinessPessoa();
        private string respCodSenha = string.Empty;

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
            Usuario user = new Usuario();

            return View(user);
        }

        [HttpPost]
        [MultipleButtonAttribute(Name = "action", Argument = "Login")]
        public ActionResult Login(Usuario model)
        {
      
            Usuario user = this.appUsuario.ValidarLogin(model);
            

            if (user == null)
                ViewBag.Validacao = "Usuario ou senha invalido";

            else if (user.Status == true)
            {
                ViewBag.Validacao = "Usuario ja encontra-se em uso";

            }

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
            Pessoa pessoa = new Pessoa();

            return View(pessoa);
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

        public ViewResult RecuperarEmail()
        {
            Usuario usuario = new Usuario();

            return View(usuario);
        }

        [HttpPost]
        [MultipleButton(Name ="action",Argument ="RecuperarEmail")]
        public ActionResult RecuperarEmail(Usuario model)
        {
            Usuario user = new Usuario()
            {
                Pessoa = model.Pessoa,
                CpfPessoa = model.CpfPessoa,
                Email = this.appUsuario.RecuperarEmail(model)
            };

            return View(user);
        }

        [HttpGet]
        public ActionResult RecuperarSenha()
        {
            Usuario usuario = new Usuario();

            ViewBag.Message = this.respCodSenha;

            return View(usuario);
        }

        [HttpPost]
        [MultipleButton(Name ="action", Argument ="RecuperarSenha")]
        public ActionResult RecuperarSenha(Usuario model)
        {
            if(model.Senha == null && model.ConfirmarSenha == null) //Pesquisar se os dados estão cadastrados no banco
            {
                Random randNum = new Random();
                string email = string.Empty;
                bool respEmail = false;

                Usuario user = new Usuario()
                {
                    Pessoa = model.Pessoa,
                    CpfPessoa = model.CpfPessoa,
                    codSenha = randNum.Next()
                };

                email = this.appUsuario.RecuperarEmail(user);

                if (email != string.Empty)
                {
                    respEmail = this.appUsuario.Enviar_Email(email);

                    if(respEmail == true)
                    {
                        ViewBag.Message = "Email enviado com sucesso";

                        return RedirectToAction("AlterarSenha",new { cpf = model.CpfPessoa });
                    }
                       
                }
                else
                    ViewBag.Message = "nao localizado";
            }
            else
            {
                ViewBag.Message = this.appUsuario.AlterarSenha(model);

                if (ViewBag.Message.Equals(""))
                    return RedirectToAction("Login");

            }

            return View(model);
        }

        [HttpGet]
        public ActionResult AlterarSenha(string cpf)
        {
            Usuario user = new Usuario()
            {
                CpfPessoa = cpf
            };

            return View(user);
        }

        [HttpPost]
        [MultipleButton(Name ="action", Argument ="AlterarSenha")]
        public ActionResult AlterarSenha(Usuario model)
        {
            string resp = this.appUsuario.AlterarSenha(model);

            if (resp.Equals("Senha alterada"))
                return RedirectToAction("Login");
            else
                ViewBag.Message = resp;
                
            return View(model);
        }
      
    }
}