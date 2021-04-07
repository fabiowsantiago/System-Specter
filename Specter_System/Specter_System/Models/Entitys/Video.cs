using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Specter_System.Models.Entitys
{
    public class Video
    {
        public string Nome { get; set; }
        public string Caminho { get; set; }
        public HttpPostedFileBase fileBaseVideo { get; set; }
        public string NomeCurso { get; set; }
    }
}
