using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Интерфейс для разбора файла
    /// </summary>
    public interface IParserFile
    {
        /// <summary>
        /// Разобрать файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        Good ParseFile(string path);
    }
}
