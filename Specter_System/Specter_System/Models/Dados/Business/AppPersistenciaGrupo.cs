using Specter_System.Models.Dados.Classes;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;

namespace Specter_System.Models.Dados.Business
{
    public class AppPersistenciaGrupo : GrupoDAO, IPGrupo
    {
        public Grupo ListarNomeGrupo()
        {
            Grupo grupo = this.SelectGrupo();

            return grupo;
        }

        public bool PesquisarGrupo(Grupo model)
        {
            bool resp = this.PesquisarNomeGrupo(model);

            return resp;
        }

        public bool Cadastrar(Grupo model)
        {
            bool resp = this.Insert(model);

            return resp;
        }

        public Grupo PesquisarQtdComponentes(Grupo model)
        {
            Grupo grupo = this.PesquisarQuantidade(model);

            return grupo;
        }

    }
}

