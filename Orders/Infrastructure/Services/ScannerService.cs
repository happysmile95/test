using System;
using System.Threading;

namespace Infrastructure
{
    /// <summary>
    /// Сервис для сканирования файлов
    /// </summary>
    public class ScannerService : IScannerService
    {
        private Timer _timer;
        private readonly int _scannerDelay = 30;
        private readonly IRepository _repository;
        private readonly IReaderFileService _readerFile;
        private readonly IFileValidatorService _fileValidator;
        private readonly IParserFileService _parserFile;
        
        public ScannerService(IReaderFileService readerFile
            , IFileValidatorService fileValidator
            , IRepository repository
            , IParserFileService parserFile)
        {
            _repository = repository;
            _readerFile = readerFile;
            _fileValidator = fileValidator;
            _parserFile = parserFile;
        }

        /// <summary>
        /// Запустить планировщик сканирования
        /// </summary>
        public void StartSchedulerScan()
        {
            _timer = new Timer(
                x => RunScan(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(_scannerDelay));
        }

        /// <summary>
        /// Запустить сканирование
        /// </summary>
        public void RunScan()
        {
            try
            {
                if (_readerFile.HasFiles())
                {
                    _readerFile.MoveFilesToProcessingFolder();
                    _fileValidator.CheckConsistencyData();

                    var goods = _parserFile.ParseFile();

                    _repository.CreateOrUpdate(goods);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, ex.Message);
            }
        }
    }
}
