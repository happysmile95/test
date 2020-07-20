using System;
using System.Collections.Generic;

namespace Infrastructure
{
    /// <summary>
    /// Репозиторий для работы в базой
    /// </summary>
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// Создать или обновить данные в базе
        /// </summary>
        /// <param name="goodDtos">Товары полученные из файлов</param>
        void CreateOrUpdate(List<GoodDto> goods);
    }
}
