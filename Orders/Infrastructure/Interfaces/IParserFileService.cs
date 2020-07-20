using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Сервис для разбора файла
    /// </summary>
    public interface IParserFileService
    {
        /// <summary>
        /// Разобрать и получить данные из файла
        /// </summary>
        /// <returns></returns>
        List<GoodDto> ParseFile();
    }
}
