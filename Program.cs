using System;
using System.IO;
using System.Collections;
using CodexNumberHandler.Interfaces;
using CodexNumberHandler.Services;

namespace CodexNumberHandler
{
    public class Program
    {
        // Минимальное и максимальное количесво генерируемых чисел в файле
        private const int MIN_NUMBERS_COUNT = 100;
        private const int MAX_NUMBERSS_COUNT = 1001;

        // Количество файлов, которые будут сгененрированы в директории
        private const int FILES_COUNT = 5;

        // Название файла, в который будет записан результат работы программы
        private const string RESULT_FILE_NAME = "result.txt";

        /// <summary>
        /// Метод, в котором начинается и завершается выполнение программы
        /// Считывает из консольной строки путь к каталогу, в котором необходимо сгенерировать 
        /// текстовые файлы. При отсутствии данного каталога создает его. Если в каталоге есть 
        /// какое-то содержимое, оно будет удалено. Генерирует данные в указанной директории.
        /// Обрабатывает данные в этом каталоге по указанным требованиям и записывает результат
        /// в файл в той же директории.
        /// В случае успеха выводит сообщение об успешной обработке данных.
        /// Если в процессе выполнения было поймано исключение, на консоль выводится сообщение
        /// об ошибке.
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter the path to the directory for generating and processing data:");
            string? dirName = Console.ReadLine();

            if (dirName is null)
            {
                Console.WriteLine("The directory path is not correct.");

                return;
            }

            try
            {
                IDataGenerator generator = DataGenerator.Builder()
                                                        .SetMinNumbersCount(MIN_NUMBERS_COUNT)
                                                        .SetMaxNumbersCount(MAX_NUMBERSS_COUNT)
                                                        .Build();
                generator.GenerateData(dirName, FILES_COUNT);

                IDataHandler dataHandler = new DataHandler();
                dataHandler.HandleDataOfDirectory(dirName, RESULT_FILE_NAME);

                Console.WriteLine($"Directory {dirName} was successfully processed!\n" +
                    $"The result was written to a file {RESULT_FILE_NAME} in the same directory.");
            } 
            catch (Exception exception)
            {
                Console.WriteLine("The process failed: " + exception.Message);
            }
        }
    }
}