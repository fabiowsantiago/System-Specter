using System;

namespace Specter_System.Models.Entitys
{
    public class Pessoa
    {
        public int PessoaId { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime Nascimento { get; set; }
        public string Profissao { get; set; }
        public string Email { get; set; }
        public string LabResp { get; set; }
    }
}
