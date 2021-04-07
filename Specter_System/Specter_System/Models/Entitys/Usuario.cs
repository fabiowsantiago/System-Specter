using System.ComponentModel.DataAnnotations;

namespace Specter_System.Models.Entitys
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string SenhaAtual { get; set; }
        public string ConfirmarSenha { get; set; }
        public string CpfPessoa { get; set; }
        public string Perfil { get; set; }
        public string Resposta { get; set; }
        public string Pessoa { get; set; }
        public bool Status { get; set; }
        public int codSenha { get; set; }

       
    }
}
