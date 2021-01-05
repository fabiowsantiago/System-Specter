using System;
using System.Web;

namespace Specter_System.Models.Entitys
{
    public class Produto
    {
        public int CursoId { get; set; }
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Palestrante { get; set; }
        public string Modalidade { get; set; }
        public DateTime Data { get; set; }
        public DateTime Data_Inicio { get; set; }
        public string Horario { get; set; }
        public string Sobre { get; set; }
        public string Informacoes { get; set; }
        public decimal Valor { get; set; }
        public string Carga_Horaria { get; set; }
        public int QuantidadeDeVagas { get; set; }
        public int QtdVendidos { get; set; }
        public int QtdDisponiveis { get; set; }
        public string LabResp { get; set; }
        public HttpPostedFileBase Imagem { get; set; }
        public HttpPostedFileBase Imagem_Palestrante { get; set; }

        public Imagem image { get; set; }
    }
}
