namespace Infrastructure
{
    /// <summary>
    /// Сервис-планировщик сканирования файлов
    /// </summary>
    public interface IScannerService
    {
        /// <summary>
        /// Запустить планировщик сканирования
        /// </summary>
        void StartSchedulerScan();
    }
}
