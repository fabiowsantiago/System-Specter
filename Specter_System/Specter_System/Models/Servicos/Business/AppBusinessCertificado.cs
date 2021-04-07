using Microsoft.Office.Interop.Word;
using Specter_System.Models.Entitys;
using Specter_System.Models.Servicos.Infaces;
using Specter_System.Validations.Impressoes;
using Specter_System.Validations.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Word = Microsoft.Office.Interop.Word;

namespace Specter_System.Models.Servicos.Business
{
    public class AppBusinessCertificado : INCertificado
    {
        private IMArquivos appArquivos = new ArquivoModel();
        public string ImprimirCertificado(Certificado model)
        {
            string resp = string.Empty;
            string fileName = "C:\\Users\\Fabio Santiago\\Desktop\\Certificados\\" + model.Aluno;

            resp = this.CreatDocument(model);
            //this.appArquivos.CreatDocument(model.Curso, model.Data, model.CargaHoraria, model.Palestrante, model.Aluno);

            if ("Arquivo criado com sucesso".Equals(resp))
            {
                if (this.Imprimir(fileName) == true){
                    resp = "Arquivo impresso";
                    File.Delete("C:\\Users\\Fabio Santiago\\Desktop\\Certificados\\"+model.Aluno+".docx");
                    File.Delete("C:\\User\\Fabio Santiago\\Desktop\\Certificados\\" + model.Aluno + ".pdf");
                };

            }
               
            return resp;
        }

        private string CreatDocument(Certificado model)
        {
            object fileName = "C:\\Users\\Fabio Santiago\\Desktop\\Certificados\\Modelo Certificado.docx";
            object SaveAs = "C:\\Users\\Fabio Santiago\\Desktop\\Certificados\\"+model.Aluno;
            string resp = string.Empty;
            Word.Application wordApp = new Word.Application();
            object missing = Missing.Value;
            Word.Document myWordDoc = null;

            if (File.Exists(fileName.ToString()))
            {
                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;

                myWordDoc = wordApp.Documents.Open(ref fileName, ref missing, ref readOnly,
                                                    ref missing, ref missing, ref missing,
                                                    ref missing, ref missing, ref missing,
                                                    ref missing, ref missing, ref missing,
                                                    ref missing, ref missing, ref missing, ref missing);

                myWordDoc.Activate();

                this.FindAndReplace(wordApp, "<aluno>", model.Aluno);
                this.FindAndReplace(wordApp, "<curso>", model.Curso);
                this.FindAndReplace(wordApp, "<data>", model.Data);
                this.FindAndReplace(wordApp, "<cargaHoraria>", model.CargaHoraria);
                this.FindAndReplace(wordApp, "<palestrante>", model.Palestrante);
            }

            else
            {
                resp = "Arquivo nao localizado";
            }

            try
            {
                myWordDoc.SaveAs2(ref SaveAs, ref missing, ref missing,
                             ref missing, ref missing, ref missing,
                             ref missing, ref missing, ref missing,
                             ref missing, ref missing, ref missing,
                             ref missing, ref missing, ref missing
                             );
                //myWordDoc = wordApp.Documents.Open("C:\\Users\\Fabio Santiago\\Desktop\\Certificados\\certificado.docx");
                myWordDoc.ExportAsFixedFormat("C:\\Users\\Fabio Santiago\\Desktop\\Certificados\\" + model.Aluno + ".pdf", WdExportFormat.wdExportFormatPDF);

                myWordDoc.Close();

                wordApp.Quit();
                resp = "Arquivo criado com sucesso";
            }
            catch(Exception error)
            {
                throw new Exception($"Error ao gerar arquivo! {error.Message}");
            }
            
            return resp;
        }

        private void FindAndReplace(Word.Application wordApp, object ToFindText, object replaceWithText)
        {
            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCard = false;
            object matchSoundLike = false;
            object matchAllForms = false;
            object forward = false;
            object format = false;
            object matchkashida = false;
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;

            wordApp.Selection.Find.Execute(ref ToFindText,
                                           ref matchCase, ref matchWholeWord,
                                           ref matchWildCard, ref matchSoundLike,
                                           ref matchAllForms, ref forward,
                                           ref wrap, ref format, ref replaceWithText,
                                           ref replace, ref matchkashida,
                                           ref matchDiactitics, ref matchAlefHamza,
                                           ref matchControl);

        }

        private bool Imprimir(string fileName)
        {
            bool resp = false;
           
            try
            {
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        Verb = "print",
                        FileName = fileName+".pdf",
                    },
                };

                process.Start();

                resp = true;

            }
            catch(Exception error)
            {
                throw new Exception($"Erro ao imprimir {error.Message}");
            }

            return resp;
        }
    }
}
