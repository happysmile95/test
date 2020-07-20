using System.IO;

namespace Infrastructure
{
    /// <summary>
    /// Хелпер для работы с папками
    /// </summary>
    public static class FolderTypeHelper
    {
        /// <summary>
        /// Директория для новых файлов
        /// </summary>
        public static readonly string InComing = "INCOMING";

        /// <summary>
        /// Директория для файлов в обработке
        /// </summary>
        public static readonly string Processed = "PROCESSED";

        /// <summary>
        /// Директория для файлов которые 
        /// в обработке завершились с ошибкой
        /// </summary>
        public static readonly string Error = "ERROR";
      
        /// <summary>
        /// Инициализация директорий
        /// </summary>
        /// <param name="currentDirectory">Текущая директория</param>
        public static void InitializeDirectories(string currentDirectory)
        {
            CreateDirectory($"{currentDirectory}\\{InComing}");
            CreateDirectory($"{currentDirectory}\\{Processed}");
            CreateDirectory($"{currentDirectory}\\{Error}");
        }

        /// <summary>
        /// Получить путь
        /// </summary>
        /// <param name="targetFolder">Целевая папка</param>
        /// <returns></returns>
        public static string GetPath(string targetFolder)
        {
            return $"{Directory.GetParent(Directory.GetCurrentDirectory())}\\{targetFolder}";
        }

        /// <summary>
        /// Создать папку
        /// </summary>
        /// <param name="path">Путь папки</param>
        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
