using Specter_System.Models.Dados.Classes;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;

namespace Specter_System.Models.Dados.Business
{
    public class AppPersistenciaModulo: ModuloDAO, IPModulo
    {
        public bool InsertModulo(Produto model)
        {
            bool resp = this.Insert_Modulo(model);

            return resp;
        }
    }
}
