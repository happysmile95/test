namespace Infrastructure
{
    /// <summary>
    /// Модель ответа файла загрузки
    /// </summary>
    public class FileUploadResult
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Результат загрузки
        /// </summary>
        public bool Success { get; set; }
    }
}
