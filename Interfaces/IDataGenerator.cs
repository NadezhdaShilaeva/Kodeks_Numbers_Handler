using System;
using System.Collections.Generic;

namespace CodexNumberHandler.Interfaces
{
    /// <summary>
    /// Интерфейс, предоставляющий методы для генерации данных
    /// </summary>
    public interface IDataGenerator
    {
        /// <summary>
        /// Метод для генерации данных в указанной директории.
        /// </summary>
        /// <param name="dirName">Путь до директории для генерации данных</param>
        /// <param name="filesCount">Количесво генерируемых файлов</param>
        void GenerateData(string dirName, int filesCount);
    }
}
