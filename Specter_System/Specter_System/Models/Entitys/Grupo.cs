using System.Collections.Generic;

namespace Specter_System.Models.Entitys
{
    public class Grupo
    {
        public string Nome { get; set; }
        public string Senha { get; set; }
        public string ConfSenha { get; set; }
        public int QtdComponentes { get; set; }
        public int QtdIntegrantes { get; set; }
        public List<Grupo> Grupos { get; set; }
        public string Resposta { get; set; }
        public string OpGrupo { get; set; }
        public string Pessoa { get; set; }
        public Produto Produto { get; set; }
    }
}
