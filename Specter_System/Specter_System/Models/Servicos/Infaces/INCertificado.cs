using Specter_System.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specter_System.Models.Servicos.Infaces
{
    public interface INCertificado
    {
        string ImprimirCertificado(Certificado model);
    }
}
