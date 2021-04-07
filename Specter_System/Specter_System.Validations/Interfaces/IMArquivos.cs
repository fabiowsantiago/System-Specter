using Specter_System.Validations.Classes;

namespace Specter_System.Validations.Interfaces
{
    public interface IMArquivos
    {
        string CreatDocument(string nomeCurso, string data, string cargaHoraria, string palestrante, string aluno);
    }
}
