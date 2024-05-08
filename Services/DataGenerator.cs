using System;
using System.IO;
using System.Collections.Generic;
using CodexNumberHandler.Interfaces;

namespace CodexNumberHandler.Services
{
    /// <summary>
    /// Класс для генерации данных, реализует интерфейс IDataGenerator
    /// </summary>
    public class DataGenerator: IDataGenerator
    {
        /// <summary>
        /// Минимальное количесво генерируемых чисел в файле включительно
        /// </summary>
        public int MinNumbersCount { get; }
        /// <summary>
        /// Максимальное количесво генерируемых чисел в файле невключительно
        /// </summary>
        public int MaxNumbersCount { get; }

        /// <summary>
        /// Минимальное значение генерируемых чисел в файле включительно
        /// </summary>
        public int MinNumberValue { get; }
        /// <summary>
        /// Максимальное значение генерируемых чисел в файле невключительно
        /// </summary>
        public int MaxNumberValue { get; }

        /// <summary>
        /// Приватный конструктор, который не доступен клиенту, но может быть вызван из 
        /// класса-билдера
        /// </summary>
        private DataGenerator(int minNumbersCount, int maxNumbersCount, 
                              int minNumberValue, int maxNumberValue)
        {
            MinNumbersCount = minNumbersCount;
            MaxNumbersCount = maxNumbersCount;
            MinNumberValue = minNumberValue;
            MaxNumberValue = maxNumberValue;
        }


        /// <summary>
        /// Статический метод для получения класса-билдера для создания объекта
        /// </summary>
        public static DataGeneratorBuilder Builder() => new DataGeneratorBuilder();

        /// <summary>
        /// Класс, реализующий паттерн builder для удобного создания объекта
        /// </summary>
        public class DataGeneratorBuilder
        {
            private int? _minNumbersCount;
            private int? _maxNumbersCount;

            private int _minNumberValue;
            private int _maxNumberValue;

            /// <summary>
            /// Конструктор класса DataGeneratorBuilder. Задает значения по умолчанию.
            /// </summary>
            public DataGeneratorBuilder()
            {
                _minNumbersCount = null;
                _maxNumbersCount = null;
                _minNumberValue = int.MinValue;
                _maxNumberValue = int.MaxValue;
            }

            /// <summary>
            /// Метод для установки минимального количества генерируемых чисел
            /// </summary>
            /// <param name="minNumbersCount">Минимальное количесво генерируемых чисел включительно</param>
            /// <returns>Тот же объект класса DataGeneratorBuilder</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// Выбрасывается при некорректном значении minNumbersCount</exception>
            public DataGeneratorBuilder SetMinNumbersCount(int minNumbersCount)
            {
                if (minNumbersCount < 0 || minNumbersCount > _maxNumbersCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(minNumbersCount));
                }

                _minNumbersCount = minNumbersCount;

                return this;
            }

            /// <summary>
            /// Метод для установки максимального количества генерируемых чисел
            /// </summary>
            /// <param name="maxNumbersCount">Максимальное количесво генерируемых чисел невключительно</param>
            /// <returns>Тот же объект класса DataGeneratorBuilder</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// Выбрасывается при некорректном значении maxNumbersCount</exception>
            public DataGeneratorBuilder SetMaxNumbersCount(int maxNumbersCount)
            {
                if (maxNumbersCount < 0 || maxNumbersCount < _minNumbersCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(maxNumbersCount));
                }

                _maxNumbersCount = maxNumbersCount;

                return this;
            }

            /// <summary>
            /// Метод для установки минимального значения генерируемых чисел
            /// </summary>
            /// <param name="minNumberValue">Минимальное значение генерируемых чисел включительно</param>
            /// <returns>Тот же объект класса DataGeneratorBuilder</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// Выбрасывается при некорректном значении minNumberValue</exception>
            public DataGeneratorBuilder SetMinNumberValue(int minNumberValue)
            {
                if (minNumberValue > _maxNumberValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(minNumberValue));
                }

                _minNumberValue = minNumberValue;

                return this;
            }

            /// <summary>
            /// Метод для установки максимального значения генерируемых чисел
            /// </summary>
            /// <param name="maxNumberValue">Максимальное значение генерируемых чисел невключительно</param>
            /// <returns>Тот же объект класса DataGeneratorBuilder</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// Выбрасывается при некорректном значении maxNumberValue</exception>
            public DataGeneratorBuilder SetMaxNumberValue(int maxNumberValue)
            {
                if (maxNumberValue < _minNumberValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(maxNumberValue));
                }

                _maxNumberValue = maxNumberValue;

                return this;
            }

            /// <summary>
            /// Метод для завершения конструирования и получения готового объекта DataGenerator
            /// </summary>
            /// <returns>Объект класса DataGenerator с указанными полями</returns>
            /// <exception cref="ArgumentNullException">Выбрасывается, если не задано минимальное 
            /// или максимально количество чисел</exception>
            public DataGenerator Build()
            {
                return new DataGenerator(
                    _minNumbersCount ?? throw new ArgumentNullException("minNumbersCount"),
                    _maxNumbersCount ?? throw new ArgumentNullException("maxNumbersCount"),
                    _minNumberValue,
                    _maxNumberValue);
            }

        }

        /// <summary>
        /// Метод для генерации данных в указанной директории. 
        /// При отсутствии данного каталога создает его. Если в каталоге есть 
        /// какое-то содержимое, оно будет удалено.
        /// </summary>
        /// <param name="dirName">Путь до директории для генерации данных</param>
        /// <param name="filesCount">Количесво генерируемых файлов</param>
        public void GenerateData(string dirName, int filesCount)
        {
            CreateEmptyDirectory(dirName);

            for (int i = 0; i < filesCount; i++)
            {
                GenerateFile(dirName);
            }
        }

        /// <summary>
        /// Метод для создания пустой директории.
        /// Если директория не существет, то она буддет создана.
        /// Если в директории есть какое-то содержимое, оно удаляется.
        /// </summary>
        /// <param name="dirName">Путь до директории</param>
        private void CreateEmptyDirectory(string dirName)
        {
            if (Directory.Exists(dirName))
            {
                Directory.Delete(dirName, true);
            }

            Directory.CreateDirectory(dirName);
        }

        /// <summary>
        /// Метод для генерации файла.
        /// С помощью Guid генерируется уникальное название файла.
        /// С помощью Random генерируется количество чисел в диапазоне от MinNumbersCount
        /// включительно до MaxNumbersCount невключительно, а также каждое число в диапазоне от 
        /// MinNumberValue включительно до MaxNumberValue невключительно и записывается в файл.
        /// </summary>
        /// <param name="dirName">Путь до директории</param>
        private void GenerateFile(string dirName)
        {
            Random random = new Random();

            string newFileName = Guid.NewGuid().ToString() + ".txt";
            string fullFilePath = Path.Combine(dirName, newFileName);

            using (StreamWriter writer = new StreamWriter(fullFilePath))
            {
                int numbersCount = random.Next(MinNumbersCount, MaxNumbersCount);
                for (int j = 0; j < numbersCount; j++)
                {
                    int number = random.Next(MinNumberValue, MaxNumberValue);
                    writer.WriteLine(number);
                }
            }
        }
    }
}
