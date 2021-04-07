using Specter_System.Models.Dados.Business;
using Specter_System.Models.Dados.Interfaces;
using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Infaces;

namespace Specter_System.Models.Servicos.Business
{
    public class AppBusinessGrupo : INGrupo
    {
        private IPGrupo appGrupo = new AppPersistenciaGrupo();
        private IPProduto appCurso = new AppPersistenciaProduto();

        public string Cadastrar(Grupo model)
        {
            bool respNome = this.appGrupo.PesquisarGrupo(model);
            string resposta = string.Empty;

            return resposta;

        }

        public Grupo ListarGrupos()
        {
            Grupo grupo = this.appGrupo.ListarNomeGrupo();

            return grupo;
        }

        public Grupo Integrar(Grupo model)
        {
            var grupo = this.appGrupo.PesquisarQtdComponentes(model);

            if (grupo == null)
            {
                grupo = new Grupo()
                {
                    Resposta = "Grupo ou senha inválido"
                };

                return grupo;
            }
            else
            {
                if (grupo.QtdIntegrantes == grupo.QtdComponentes)
                {
                    grupo = new Grupo()
                    {
                        Resposta = "Grupo encontra-se completo"
                    };

                    return grupo;

                }

                else
                {
                    model = new Grupo()
                    {
                        Nome = model.Nome,
                        QtdIntegrantes = grupo.QtdIntegrantes++
                    };


                    return grupo;
                }
            }

        }
    }
}
