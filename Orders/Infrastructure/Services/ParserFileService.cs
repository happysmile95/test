using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис для разбора файла
    /// </summary>
    public class ParserFileService : IParserFileService
    {
        /// <summary>
        /// Разобрать и получить данные из файла
        /// </summary>
        /// <returns></returns>
        public List<GoodDto> ParseFile()
        {
            List<GoodDto> goods = new List<GoodDto>();
            //ExcelPackage.LicenseContext = LicenseContext.
            foreach (var file in Directory.GetFiles(FolderTypeHelper.GetPath(FolderTypeHelper.Processed)).Select(e => new FileInfo(e)))
            {
                using (var package = new ExcelPackage(file))
                {
                    foreach (var worksheet in package.Workbook.Worksheets)
                    {
                        goods.AddRange(ParseFile(worksheet, worksheet.Dimension.Rows));
                    }
                }
            }

            return goods;
        }

        /// <summary>
        /// Разобрабть и получить данные
        /// </summary>
        /// <param name="worksheet">Лист excel</param>
        /// <param name="rows">Номер строки</param>
        /// <returns></returns>
        private List<GoodDto> ParseFile(ExcelWorksheet worksheet, int rows)
        {
            List<GoodDto> goods = new List<GoodDto>();
            for (int i = 2; i <= rows; i++)
            {
                goods.Add(GetGood(worksheet, i));
            }
            return goods;
        }

        /// <summary>
        /// Получить товар
        /// </summary>
        /// <param name="worksheet">Лист excel</param>
        /// <param name="row">Номер строки</param>
        /// <returns></returns>
        private GoodDto GetGood(ExcelWorksheet worksheet, int row)
        {
            int cell = 0;
            return new GoodDto()
            {
                SoldTo = worksheet.Cells[row, ++cell].Value.ToString(),
                CustName = worksheet.Cells[row, ++cell].Value.ToString(),
                ShipTo = worksheet.Cells[row, ++cell].Value.ToString(),
                ShipToNa = worksheet.Cells[row, ++cell].Value.ToString(),
                OrderType = worksheet.Cells[row, ++cell].Value.ToString(),
                Dv = worksheet.Cells[row, ++cell].Value.ToString(),
                OrderNum = worksheet.Cells[row, ++cell].Value.ToString(),
                Material = worksheet.Cells[row, ++cell].Value.ToString(),
                MatDes = worksheet.Cells[row, ++cell].Value.ToString(),
                Size = worksheet.Cells[row, ++cell].Value.ToString(),
                AltSize = worksheet.Cells[row, ++cell].Value.ToString(),
                OnOrdQty = int.Parse(worksheet.Cells[row, ++cell].Value.ToString()),
                ShipQty = int.Parse(worksheet.Cells[row, ++cell].Value.ToString()),
                RejectQty = int.Parse(worksheet.Cells[row, ++cell].Value.ToString())
            };
        }
    }
}
