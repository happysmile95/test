using Data;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Сервис для валидация файла
    /// </summary>
    public class FileValidatorService : IFileValidatorService
    {
        /// <summary>
        /// Сервис для чтения файла
        /// </summary>
        private readonly IReaderFileService _readerFileService;

        public FileValidatorService(IReaderFileService readerFileService)
        {
            _readerFileService = readerFileService;
        }

        /// <summary>
        /// Проверить консистентность данных
        /// </summary>
        public void CheckConsistencyData()
        {
            try
            {
                foreach (var file in Directory.GetFiles(FolderTypeHelper.GetPath(FolderTypeHelper.Processed)).Select(e => new FileInfo(e)))
                {
                    using (var package = new ExcelPackage(file))
                    {
                        foreach (var worksheet in package.Workbook.Worksheets)
                        {
                            ValidationData(worksheet, worksheet.Dimension.Start.Row, worksheet.Dimension.End.Column);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
                _readerFileService.MoveFileToErrorFolder();
                throw ex;
            }
        }

        /// <summary>
        /// Проверить загруженный файл
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        public string ValidateUploadedFile(IFormFile file)
        {
            if (file == null)
            {
                return "Файл не выбран.";
            }
            if (!file.FileName.Contains("xlsx"))
            {
                return "Некорректный файл. Необходим файл с расширением *.xlxs";
            }
            if (file.Length == 0)
            {
                return "Файл пуст";
            }
            return null; 
        }

        /// <summary>
        /// Проверить консистентность данных
        /// </summary>
        /// <param name="worksheet">Лист excel</param>
        /// <param name="i">Номер строки</param>
        /// <param name="j">Номер столбца</param>
        private void ValidationData(ExcelWorksheet worksheet, int rows, int columns)
        {
            for (int i = 2; i <= rows; i++)
            {
                for (int j = 1; j <= columns; j++)
                {
                    if (!ValidationValue(worksheet, i, j))
                    {
                        throw new ApplicationException("The data is not consistent");
                    }
                }
            }
        }

        /// <summary>
        /// Проверить консистентность значения
        /// </summary>
        /// <param name="worksheet">Лист excel</param>
        /// <param name="i">Номер строки</param>
        /// <param name="j">Номер столбца</param>
        /// <returns></returns>
        private bool ValidationValue(ExcelWorksheet worksheet, int i, int j)
        {
            try
            {
                switch (j)
                {
                    case 12:
                    case 13:
                    case 14:
                        {
                            return int.TryParse(worksheet.Cells[i, j].Value.ToString(), out int value) && value >= 0;
                        }
                    default:
                        {
                            return !string.IsNullOrEmpty(worksheet.Cells[i, j].Value.ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
