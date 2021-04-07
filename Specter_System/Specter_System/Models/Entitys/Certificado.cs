using System.Collections.Generic;

namespace Specter_System.Models.Entitys
{
    public class Certificado
    {
        public int CertificadoId { get; set; }
        public string Aluno { get; set; }
        public string Curso { get; set; }
        public string Data { get; set; }
        public string Horario { get; set; }
        public string CargaHoraria { get; set; }
        public string Palestrante { get; set; }
        public List<Certificado> Certificados {get;set;}
    }
}
