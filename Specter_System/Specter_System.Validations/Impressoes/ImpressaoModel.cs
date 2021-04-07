using Specter_System.Validations.Interfaces;
using System.Diagnostics;

namespace Specter_System.Validations.Impressoes
{
    public class ImpressaoModel : IMImpressao
    {
        public bool Imprimir(string fileName)
        {
            bool resp = false;
            string error = string.Empty;

            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        Verb = "print",
                        FileName = fileName + ".pdf",
                    },
                };

                process.Start();

                resp = true;

            }
            catch
            {
                error = "Erro ao imprimir";
            }

            return resp;
        }
    }
}
