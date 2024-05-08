using System;

namespace CodexNumberHandler.Interfaces
{
    /// <summary>
    /// Интерфейс, предоставляющий методы для обработки данных
    /// </summary>
    public interface IDataHandler
    {
        /// <summary>
        /// Метод для обработки данных в указанной директории и записи результата в файл 
        /// в той же директории.
        /// </summary>
        /// <param name="fromDirName">Путь до директории, в которой требуется обработать данные</param>
        /// <param name="toFileName">Название файла для записи результата</param>
        void HandleDataOfDirectory(string fromDirName, string toFileName);
    }
}
