namespace Infrastructure
{
    /// <summary>
    /// Планировщик сканирования
    /// </summary>
    public class StartSchedulerScanner
    {
        /// <summary>
        /// Сервис-планировщик сканирования файлов
        /// </summary>
        private readonly IScannerService _scannerService;

        public StartSchedulerScanner(IScannerService scannerService)
        {
            _scannerService = scannerService;
        }

        /// <summary>
        /// Запустить планировщик
        /// </summary>
        public void RunScheduller()
        {
            _scannerService.StartSchedulerScan();
        }
    }
}
