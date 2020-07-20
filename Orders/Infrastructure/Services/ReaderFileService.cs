using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.IO;
using System.Linq;

namespace Infrastructure
{
    /// <summary>
    /// Сервис для чтения с файла
    /// </summary>
    public class ReaderFileService : IReaderFileService
    {
        /// <summary>
        /// Проверка файлов на наличии в директории
        /// </summary>
        /// <returns></returns>
        public bool HasFiles()
        {
            return Directory.GetFiles(FolderTypeHelper.GetPath(FolderTypeHelper.InComing)).Any();
        }

        /// <summary>
        /// Перенос файла из папки в папку
        /// </summary>
        public void MoveFilesToProcessingFolder()
        {
            foreach (var file in Directory.GetFiles(FolderTypeHelper.GetPath(FolderTypeHelper.InComing)))
            {
                try
                {
                    File.Move(file, string.Format("{0}\\{1}", FolderTypeHelper.GetPath(FolderTypeHelper.Processed), new FileInfo(file).Name));
                }
                catch(Exception ex)
                {
                    Serilog.Log.Error(ex, ex.Message);
                    RollbackFiles(FolderTypeHelper.GetPath(FolderTypeHelper.Processed), FolderTypeHelper.GetPath(FolderTypeHelper.InComing));
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Откат файлов в исходную папку
        /// </summary>
        /// <param name="dirFrom">Откуда</param>
        /// <param name="dirTo">Куда</param>
        public void RollbackFiles(string dirFrom, string dirTo)
        {
            try
            {
                foreach (var file in Directory.GetFiles(dirFrom).Where(e => (new FileInfo(e).CreationTime.Minute - DateTime.Now.Minute) < 5))
                {
                    File.Move(file, string.Format("{0}\\{1}", dirTo, new FileInfo(file).Name));
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }
            
        }

        /// <summary>
        /// Перенос ошибочных файлов
        /// </summary>
        public void MoveFileToErrorFolder()
        {
            foreach (var file in Directory.GetFiles(FolderTypeHelper.GetPath(FolderTypeHelper.Processed)))
            {
                File.Move(file, string.Format("{0}\\{1}", FolderTypeHelper.GetPath(FolderTypeHelper.Error), new FileInfo(file).Name));
            }
        }
    }
}
