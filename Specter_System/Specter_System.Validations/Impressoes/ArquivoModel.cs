using Microsoft.Office.Interop.Word;
using Specter_System.Validations.Classes;
using Specter_System.Validations.Interfaces;
using System;
using System.IO;
using System.Reflection;
using Word = Microsoft.Office.Interop.Word;

namespace Specter_System.Validations.Impressoes
{
    public class ArquivoModel : IMArquivos
    {
        public string CreatDocument(string nomeCurso, string data, string cargaHoraria, string palestrante, string aluno)
        {
            object fileName = "C:\\Users\\Fabio Santiago\\Desktop\\Certificados\\Modelo Certificado.docx";
            object SaveAs = "C:\\Users\\Fabio Santiago\\Desktop\\Certificados\\"+aluno;
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

                this.FindAndReplace(wordApp, "<aluno>","FABIO WELBER DA SILVA SANTIAGO");
                this.FindAndReplace(wordApp, "<curso>", nomeCurso);
                this.FindAndReplace(wordApp, "<cargaHoraria>", cargaHoraria);
                this.FindAndReplace(wordApp, "<data>", data);
                this.FindAndReplace(wordApp, "<palestrante>", palestrante);
                
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
                myWordDoc = wordApp.Documents.Open("C:\\Users\\Fabio Santiago\\Desktop\\Certificados\\" + SaveAs + ".docx");
                myWordDoc.ExportAsFixedFormat("C:\\Users\\Fabio Santiago\\Desktop\\Certificados\\" + SaveAs + ".docx", WdExportFormat.wdExportFormatPDF);

                // myWordDoc.Close();

                wordApp.Quit();
                //File.Delete("C:\\Users\\Fabio Santiago\\Desktop\\Certificados\\"+model.Curso+".docx");
                resp = "Arquivo criado com sucesso";
            }
            catch(Exception error)
            {
                resp = $"Error ao gerar arquivo!{error.Message}";
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

    }
}
