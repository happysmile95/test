using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Интерфейс для работы с файлом
    /// </summary>
    public interface IReaderFile : IDisposable
    {
        /// <summary>
        /// Прочитать файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        void ReadFile(string path);
    }
}
