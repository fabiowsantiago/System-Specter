using Specter_System.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specter_System.Models.Servicos.Infaces
{
    public interface INArquivo
    {
        bool Salvar_Imagem_Curso_Online(Produto model);
        string Salvar_Videos(Produto model);
    }
}
