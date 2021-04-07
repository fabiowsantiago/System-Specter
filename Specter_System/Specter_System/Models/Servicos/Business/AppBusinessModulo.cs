using Specter_System.Models.Dados.Business;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Infaces;

namespace Specter_System.Models.Servicos.Business
{
    public class AppBusinessModulo : INModulo
    {
        private IPModulo appModulo = new AppPersistenciaModulo();

        public string SalvarModulos(Produto model)
        {
            string resp = string.Empty;
            bool respModulo = this.appModulo.InsertModulo(model);

            if (respModulo == true)
                resp = "Salvo";
            else
                resp = "Erro ao salvar curso";

            return resp;
        }

    }
}
