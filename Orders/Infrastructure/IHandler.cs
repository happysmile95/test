using System;
using Data;

namespace Infrastructure
{
    /// <summary>
    /// Интерфейс для обработки записей с базой данных
    /// </summary>
    public interface IHandler : IDisposable
    {
        /// <summary>
        /// Записать заказ в базу
        /// </summary>
        /// <param name="good">Заказ</param>
        void WriteOrder(Good good);
    }
}
