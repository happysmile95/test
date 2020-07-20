namespace Infrastructure
{
    /// <summary>
    /// Сервис для чтения с файла
    /// </summary>
    public interface IReaderFileService
    {
        /// <summary>
        /// Проверка файлов на наличии в директории
        /// </summary>
        /// <returns></returns>
        bool HasFiles();

        /// <summary>
        /// Перенос файла из папки в папку
        /// </summary>
        void MoveFilesToProcessingFolder();

        /// <summary>
        /// Откат файлов в исходную папку
        /// </summary>
        /// <param name="dirFrom">Откуда</param>
        /// <param name="dirTo">Куда</param>
        void RollbackFiles(string dirFrom, string dirTo);

        /// <summary>
        /// Перенос ошибочных файлов
        /// </summary>
        void MoveFileToErrorFolder();
    }
}
